namespace Lamashare.CLI.Services.Command.Commands.Login;

public class LoginCommand(IServiceProvider serviceProvider) : ICommand
{
    public string GetName()
    {
        return "login";
    }

    public Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<LoginOptions>(args);
        if (result.Errors.Any()) return Task.FromResult(1);

        return Task.FromResult(0);
    }
}