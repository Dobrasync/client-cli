using CommandLine;
using Lamashare.CLI.Services.Command.Commands;
using Lamashare.CLI.Storage.Service;

namespace Lamashare.CLI.Services.Command;

public class CommandService(ILoggerService logger) : ICommandService
{
    List<ICommand> commands = new List<ICommand>()
    {
        new CloneCommand(),
        new RemoveCommand(),
        new LoginCommand(),
        new LogoutCommand(),
    };
    public void Consume(ParserResult<Lamashare.CLI.Storage.Arguments.Options> options)
    {
        ICommand? command = commands.FirstOrDefault(x => x.GetName().Equals(options.Value.Action, StringComparison.OrdinalIgnoreCase));
        if (command == null)
        {
            logger.LogFatal($"Invalid command. Available commands are: {string.Join(", ", commands.Select(x => x.GetName()).ToArray())}");
            Environment.Exit(1);
        }
    }
}