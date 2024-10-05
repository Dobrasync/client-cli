using Lamashare.CLI.Const;

namespace Lamashare.CLI.Services.Command.Commands.Sync;

public class ScanCommand(ISyncService syncService, ILoggerService logger) : ICommand
{
    public string GetName()
    {
        return "scan";
    }

    public async Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<ScanOptions>(args);
        if (result.Errors.Any()) return 1;

        if (result.Value.SyncAll)
        {
            return await syncService.SyncAllLibraries();
        }

        if (result.Value.LibraryId == null)
        {
            logger.LogError("Library id is required.");
            return ExitCodes.Failure;
        }

        return await syncService.SyncLibrary(result.Value.LibraryId ?? new Guid());
    }
}