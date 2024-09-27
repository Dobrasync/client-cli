namespace Lamashare.CLI.Services.Command;

public class CommandService(IServiceProvider serviceProvider) : ICommandService
{
    List<ICommand> commands = new List<ICommand>()
    {
        ActivatorUtilities.CreateInstance<LoginCommand>(serviceProvider),
        ActivatorUtilities.CreateInstance<CloneCommand>(serviceProvider),
    };
    public async Task<int> Consume(string[] args)
    {
        if (args.Length == 0) return 1;
        ICommand? commandMatch = commands.FirstOrDefault(x => x.GetName().Equals(args[0], StringComparison.OrdinalIgnoreCase));
        if (commandMatch == null) return 1;
        
        int exitCode = await commandMatch.Execute(args);
        return exitCode;
    }
    
}