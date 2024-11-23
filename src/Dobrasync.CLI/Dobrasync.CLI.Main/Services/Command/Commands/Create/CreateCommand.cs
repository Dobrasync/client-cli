using Lamashare.CLI.Const;
using Lamashare.CLI.Db.Enums;
using Lamashare.CLI.Services.SystemSetting;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Lamashare.CLI.Services.Command.Commands.Create;

public class CreateCommand(ISyncService syncService) : ICommand
{
    public string GetName()
    {
        return "create";
    }

    public async Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<CreateOptions>(args);
        if (result.Errors.Any()) return ExitCodes.Failure;

        return await syncService.CreateLibrary(result.Value.Name);
    }
}