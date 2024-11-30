using Lamashare.CLI.Const;
using Lamashare.CLI.Services.Command.Commands.Configure;
using Lamashare.CLI.Services.Command.Commands.Create;
using Lamashare.CLI.Services.Command.Commands.List;
using Lamashare.CLI.Services.Command.Commands.Logout;
using Lamashare.CLI.Services.Command.Commands.Remove;
using Lamashare.CLI.Services.Command.Commands.Stats;
using Lamashare.CLI.Services.Command.Commands.Sync;

namespace Lamashare.CLI.Services.Command;

public class CommandService(IServiceProvider serviceProvider, ILoggerService logger) : ICommandService
{
    List<ICommand> commands = new()
    {
        ActivatorUtilities.CreateInstance<LoginCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<LogoutCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<CloneCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<RemoveCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<ListCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<ConfigureCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<SyncCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<StatsCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<CreateCommand>(serviceProvider),
    };
    public async Task<int> Consume(string[] args)
    {
        if (args.Length == 0)
        {
            logger.LogFatal($"No command provided. Valid commands are: {string.Join(", ", commands.Select(x => x.GetName()).ToArray())}");
            return ExitCodes.Failure;
        }
        
        ICommand? commandMatch = commands.FirstOrDefault(x => x.GetName().Equals(args[0], StringComparison.OrdinalIgnoreCase));
        if (commandMatch == null)
        {
            logger.LogFatal($"Invalid command '{args[0]}'. Valid commands are: {string.Join(", ", commands.Select(x => x.GetName()).ToArray())}");
            return ExitCodes.Failure;
        }
        
        int exitCode = await commandMatch.Execute(args);
        return exitCode;
    }
    
}