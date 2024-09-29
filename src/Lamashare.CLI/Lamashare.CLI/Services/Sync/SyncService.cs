using System.Net;
using System.Runtime.InteropServices;
using Lamashare.CLI.ApiGen.Mainline;
using Lamashare.CLI.Const;
using Lamashare.CLI.Db.Entities;
using Lamashare.CLI.Db.Enums;
using Lamashare.CLI.Services.SystemSetting;
using Lamashare.CLI.Shared.Exceptions;
using LamashareApi.Database.Repos;
using LamashareCore;
using LamashareCore.Util;

namespace Lamashare.CLI.Services.Sync;

public class SyncService(IApiClient apiClient, IRepoWrapper repoWrap, ILoggerService logger, ISystemSettingService settings) : ISyncService
{
    #region Login
    public Task<int> Login()
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Logout
    public Task<int> Logout()
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Clone
    public async Task<int> CloneLibrary(Guid remoteLibraryId, string? localLibraryPath)
    {
        #region Check if already cloned
        bool alreadyCloned = await IsLibraryCloned(remoteLibraryId);
        if (alreadyCloned)
        {
            logger.LogError("Library has already been cloned.");
            return 1;
        }
        #endregion
        #region Load required data

        LibraryDto? remoteLibrary = null;
        try
        {
            remoteLibrary = await apiClient.GetLibraryByIdAsync(remoteLibraryId);
        }
        catch (ApiException e)
        {
            logger.LogError(e.Message);
            return 1;
        }
        
        if (remoteLibrary == null)
        {
            logger.LogError("Library with given id does not exist.");
            return 1;
        }
        #endregion
        
        #region Create library directory in default dir
        if (localLibraryPath == null)
        {
            var def = await settings.TryGetSettingAsync(ESystemSetting.DEFAULT_LIBRARY_DIRECTORY);
            if (def == null)
            {
                logger.LogError("Default library path is not configured.");
                return 1;
            }

            if (string.IsNullOrEmpty(def.Value))
            {
                logger.LogError($"The default library path is invalid: '{def.Value}'");
                return 1;
            }
            
            localLibraryPath = Path.Join(def.Value, remoteLibrary.Name);
            try
            {
                Directory.CreateDirectory(localLibraryPath);
            }
            catch (Exception e)
            {
                logger.LogError($"Could not create a new directory for library: {e.Message}");
                logger.LogDebug($"Stack trace: {e.StackTrace}");
                return 1;
            }
        };
        #endregion
        
        #region Throw if locallibrarypath does not exist
        if (!Directory.Exists(localLibraryPath))
        {
            logger.LogError("Local library path does not exist or is not a directory.");
            return 1;
        }
        #endregion
        
        #region Create db entry
        var localLib = new Library()
        {
            LocalPath = localLibraryPath,
            RemoteId = remoteLibraryId,
        };
        await repoWrap.LibraryRepo.InsertAsync(localLib);
        #endregion
        
        logger.LogInfo("Library has been cloned.");
        return 0;
    }
    #endregion
    #region Remove
    public async Task<int> RemoveLibrary(Guid localLibraryId, bool deleteDirectory = false)
    {
        #region Check if library is cloned
        var lib = await repoWrap.LibraryRepo.GetByIdAsync(localLibraryId);
        if (lib == null)
        {
            logger.LogError("Library with given id does not exist.");
            return ExitCodes.Failure;
        }
        #endregion
        
        #region Remove from DB
        await repoWrap.LibraryRepo.DeleteAsync(lib);
        logger.LogInfo("Library has been removed.");
        #endregion
        
        #region Delete directory
        if (deleteDirectory && !string.IsNullOrEmpty(lib.LocalPath))
        {
            Directory.Delete(lib.LocalPath, true);
            logger.LogInfo("Library directory deleted.");
        }
        #endregion
        
        return ExitCodes.Success;
    }
    #endregion
    
    #region Sync all
    public async Task<int> SyncAllLibraries()
    {
        var libs= await repoWrap.LibraryRepo.QueryAll().ToListAsync();
        if (libs.Count == 0)
        {
            logger.LogInfo("No libraries to sync.");
            return ExitCodes.Success;
        }
        
        foreach (var lib in libs)
        {
            await SyncLibrary(lib.Id);
        }
        return ExitCodes.Success;
    }
    #endregion
    #region Sync
    public async Task<int> SyncLibrary(Guid localLibId)
    {
        logger.LogInfo($"Invoking sync for {localLibId}...");
        #region load
        var lib = await repoWrap.LibraryRepo.GetByIdAsync(localLibId);
        if (lib == null)
        {
            logger.LogError("Library with given id does not exist.");
            return ExitCodes.Failure;
        }
        #endregion
        
        #region make file list
        logger.LogInfo("Generating local library file tree...");
        var localFiles = Directory.GetFiles(lib.LocalPath, "*.*", SearchOption.AllDirectories);
        List<FileInfoDto> lfi = new();
        foreach (string syspath in localFiles)
        {
            FileInfo info = new(syspath);
            string libpath = FileUtil.FileSysPathToLibPath(syspath, lib.LocalPath);
            
            lfi.Add(new()
            {
                LibraryId = localLibId,
                ModifiedOn = info.LastWriteTimeUtc,
                TotalChecksum = await FileUtil.GetFileTotalChecksumAsync(syspath),
                FileLibraryPath = libpath,
            });
        }
        #endregion
        
        #region Get diff list
        logger.LogInfo("Getting diff from remote...");
        var diff = await apiClient.GetDiffAsync(new()
        {
            LibraryId = lib.RemoteId,
            FilesOnLocal = lfi
        });
        #endregion
        
        #region Pull
        logger.LogInfo("Pulling remote files...");
        foreach (var file in diff.RequiredByLocal)
        {
            var assembledBlocks = new List<byte[]>();
            var remoteBlocklist = await apiClient.GetFileBlockListAsync(lib.RemoteId, file);
            
            #region Begin transaction
            var transaction = await apiClient.CreateFileTransactionAsync(new()
            {
                LibraryId = localLibId,
                FileLibraryPath = file,
                Type = EFileTransactionType.PULL,
                BlockChecksums = null,
                TotalChecksum = null,
            });
            #endregion
            
            foreach (var block in remoteBlocklist)
            {
                BlockDto pulled = await apiClient.PullBlockAsync(block);
                assembledBlocks.Add(pulled.Content);
            }
            
            var rawBytes = assembledBlocks.SelectMany(x => x).ToArray();
            using (FileStream fileStream = new FileStream(FileUtil.FileLibPathToSysPath(file, lib.LocalPath), FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(rawBytes, 0, rawBytes.Length);
            }
            
            #region Finalize transaction
            var result = await apiClient.FinishFileTransactionAsync(transaction.TransactionId);
            #endregion
        }
        #endregion
        
        #region Push
        logger.LogInfo("Pushing local files...");
        foreach (var file in diff.RequiredByRemote)
        {
            List<BlockDto> blocks = new();
            var assembledBlocks = new List<byte[]>();
            var remoteBlocklist = await apiClient.GetFileBlockListAsync(lib.RemoteId, file);
            var localBlocklist = FileUtil.GetFileBlocks(FileUtil.FileLibPathToSysPath(file, lib.LocalPath));
            string totalChecksum = await FileUtil.GetFileTotalChecksumAsync(FileUtil.FileLibPathToSysPath(file, lib.LocalPath));
            
            #region Begin transaction
            var transaction = await apiClient.CreateFileTransactionAsync(new()
            {
                LibraryId = localLibId,
                FileLibraryPath = file,
                Type = EFileTransactionType.PUSH,
                BlockChecksums = localBlocklist.Select(x => x.Checksum).ToArray(),
                TotalChecksum = totalChecksum,
            });
            #endregion
            
            foreach (var block in localBlocklist)
            {
                var remoteBlock = remoteBlocklist.FirstOrDefault(x => x == block.Checksum);
                // If the local block is not present on remote, push it
                if (remoteBlock == null)
                {
                    await apiClient.PushBlockAsync(new BlockDto()
                    {
                        Checksum = block.Checksum,
                        Content = block.Payload
                    });
                }
            }
            
            #region finalize transaction
            await apiClient.FinishFileTransactionAsync(transaction.TransactionId);
            #endregion
        }
        #endregion

        return 0;
    }
    #endregion
    
    #region Utility
    private async Task<bool> IsLibraryCloned(Guid remoteLibraryId)
    {
        var match = await repoWrap.LibraryRepo.QueryAll().FirstOrDefaultAsync(x => x.RemoteId == remoteLibraryId);
        return match != null;
    }
    
    #endregion
}