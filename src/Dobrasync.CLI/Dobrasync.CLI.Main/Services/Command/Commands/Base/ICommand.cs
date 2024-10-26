namespace Lamashare.CLI.Services.Command.Commands.Base;

public interface ICommand
{
    public string GetName();
    public Task<int> Execute(string[] args);
}