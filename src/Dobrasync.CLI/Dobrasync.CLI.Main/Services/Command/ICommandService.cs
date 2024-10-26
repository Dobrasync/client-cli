namespace Lamashare.CLI.Services.Command;

public interface ICommandService
{
    public Task<int> Consume(string[] args);
}