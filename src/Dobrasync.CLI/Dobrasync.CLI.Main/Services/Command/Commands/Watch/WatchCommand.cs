namespace Lamashare.CLI.Services.Command.Commands.Watch;

public class WatchCommand : ICommand
{
    public string GetName()
    {
        return "watch";
    }

    public Task<int> Execute(string[] args)
    {
        throw new NotImplementedException();
    }
}