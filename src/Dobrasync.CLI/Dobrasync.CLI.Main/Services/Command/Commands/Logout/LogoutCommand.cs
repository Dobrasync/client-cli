using Lamashare.CLI.Const;
using Lamashare.CLI.Services.Auth;

namespace Lamashare.CLI.Services.Command.Commands.Logout;

public class LogoutCommand(IAuthService authService) : ICommand
{
    public string GetName()
    {
        return "logout";
    }

    public async Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<LoginOptions>(args);
        if (result.Errors.Any()) return ExitCodes.Failure;

        await authService.LogoutAsync();

        return ExitCodes.Success;
    }
}