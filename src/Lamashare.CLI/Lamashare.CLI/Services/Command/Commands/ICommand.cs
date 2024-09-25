namespace Lamashare.CLI.Services.Command.Commands;

public interface ICommand
{
    public string GetName();
    public void Execute();
}