using Lamashare.CLI.ApiGen.Mainline;
using Lamashare.CLI.Const;
using Lamashare.CLI.Services.Auth;

namespace Lamashare.CLI.Services.Command.Commands.Login;

public class LoginCommand(IServiceProvider serviceProvider, IAuthService authService) : ICommand
{
    public string GetName()
    {
        return "login";
    }

    public async Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<LoginOptions>(args);
        if (result.Errors.Any()) return ExitCodes.Failure;

        await authService.Authenticate();

        return ExitCodes.Success;
    }
}