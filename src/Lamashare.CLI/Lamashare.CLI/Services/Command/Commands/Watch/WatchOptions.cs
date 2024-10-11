namespace Lamashare.CLI.Services.Command.Commands.Sync;


[Verb("watch", HelpText = "Watch a library directory for changes and sync automatically.")]
public class WatchOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
    
    [Value(1, MetaName = "Library id", HelpText = "Id of the local library to watch", Required = true)]
    public Guid LibraryId { get; set; }

    [Option('r', "remove", HelpText = "Removed the library from watchlist.", Default = false, Required = false)]
    public bool Remove { get; set; } = false;
}