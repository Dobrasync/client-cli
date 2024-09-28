using System.Text;
using System.Text.Json;
using LamashareApi.Database.Repos;

namespace Lamashare.CLI.Services.Command.Commands.List;

public class ListCommand(IRepoWrapper repoWrap, ILoggerService logger) : ICommand
{
    public string GetName()
    {
        return "ls";
    }

    public async Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<ListOptions>(args);
        if (result.Errors.Any()) return 1;

        logger.LogDebug("Loading list...");
        var libs = await repoWrap.LibraryRepo.QueryAll().ToListAsync();
        
        logger.LogInfo(JsonSerializer.Serialize(libs));
        return 0;
    }
}