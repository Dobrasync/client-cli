using Lamashare.CLI.ApiGen.Mainline;
using Lamashare.CLI.Const;
using Lamashare.CLI.Services.Auth;

namespace Lamashare.CLI.Services.Command.Commands.Login;

public class LoginCommand(IServiceProvider serviceProvider, IAuthService authService, ILoggerService logger) : ICommand
{
    public string GetName()
    {
        return "login";
    }

    public async Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<LoginOptions>(args);
        if (result.Errors.Any()) return ExitCodes.Failure;

        try
        {
            await authService.AuthenticateAsync();
        }
        catch (Exception e)
        {
            logger.LogFatal($"Failed to authenticate: {e.Message}");
            logger.LogDebug($"Stack Trace: {e.StackTrace}");
            return ExitCodes.Failure;
        }
        

        return ExitCodes.Success;
    }
}