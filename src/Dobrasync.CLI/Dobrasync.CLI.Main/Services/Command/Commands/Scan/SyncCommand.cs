using Lamashare.CLI.Const;

namespace Lamashare.CLI.Services.Command.Commands.Sync;

public class SyncCommand(ISyncService syncService, ILoggerService logger) : ICommand
{
    public string GetName()
    {
        return "sync";
    }

    public async Task<int> Execute(string[] args)
    {
        DateTimeOffset startTime = DateTimeOffset.UtcNow;
        var result = Parser.Default.ParseArguments<SyncOptions>(args);
        if (result.Errors.Any()) return 1;

        if (result.Value.SyncAll)
        {
            int c = await syncService.SyncAllLibraries();
            logger.LogInfo($"Sync took {(DateTimeOffset.UtcNow - startTime).TotalSeconds} seconds.");
            return c;
        }

        if (result.Value.LibraryId == null)
        {
            logger.LogError("Library id is required.");
            return ExitCodes.Failure;
        }

        int exitCode = await syncService.SyncLibrary(result.Value.LibraryId ?? new Guid());

        logger.LogInfo($"Sync took {(DateTimeOffset.UtcNow - startTime).TotalSeconds} seconds.");
        return exitCode;
    }
}