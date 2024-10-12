using Lamashare.CLI.ApiGen.Mainline;
using Lamashare.CLI.Db.Entities;

namespace Lamashare.CLI.Services.Command.Commands.Clone;

public class CloneCommand(ILoggerService logger, IApiClient apiClient, ISyncService syncService) : ICommand
{
    public string GetName()
    {
        return "clone";
    }

    public async Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<CloneOptions>(args);
        if (result.Errors.Any()) return 1;

        int code = await Clone(result);
        
        return 0;
    }

    private async Task<int> Clone(ParserResult<CloneOptions> results)
    {
        Guid libraryId = results.Value.LibraryId;
        string localLibraryPath = results.Value.LocalLibraryPath;
        
        await syncService.CloneLibrary(libraryId, localLibraryPath);
        
        return 0;
    }
}