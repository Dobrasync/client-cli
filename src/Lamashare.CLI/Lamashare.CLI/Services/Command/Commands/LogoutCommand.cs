namespace Lamashare.CLI.Services.Command.Commands;

public class LogoutCommand : ICommand
{
    public string GetName()
    {
        return "logout";
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }
}