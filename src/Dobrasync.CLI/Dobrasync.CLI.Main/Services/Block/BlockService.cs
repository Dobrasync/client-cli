using Lamashare.CLI.Db.Enums;
using Lamashare.CLI.Services.SystemSetting;
using LamashareCore.Util;

namespace Lamashare.CLI.Services.Block;

public class BlockService(ISystemSettingService settings) : IBlockService
{
    public async Task WriteTempBlock(string checksum, byte[] content)
    {
        var tempBlockDir = await settings.GetSettingValueThrowsAsync(ESystemSetting.TEMP_BLOCK_DIRECTORY);
        if (!Directory.Exists(tempBlockDir))
        {
            Directory.CreateDirectory(tempBlockDir);
        }
        
        await File.WriteAllBytesAsync(Path.Combine(tempBlockDir, checksum), content);
    }

    public async Task RestoreFileFromBlocks(List<string> blocks, string targetPath, DateTime lastModified,
        DateTime creationTime)
    {
        var tempBlockDir = await settings.GetSettingValueThrowsAsync(ESystemSetting.TEMP_BLOCK_DIRECTORY);
        var blockPaths = blocks.Select(x => Path.Join(tempBlockDir, x)).ToList();
        await FileUtil.FullRestoreFileFromBlocks(blockPaths, targetPath, lastModified, creationTime);
    }
}