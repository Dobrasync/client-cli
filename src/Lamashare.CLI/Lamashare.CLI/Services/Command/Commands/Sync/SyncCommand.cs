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
        var result = Parser.Default.ParseArguments<SyncOptions>(args);
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