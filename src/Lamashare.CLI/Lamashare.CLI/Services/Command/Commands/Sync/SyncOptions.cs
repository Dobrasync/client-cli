namespace Lamashare.CLI.Services.Command.Commands.Sync;


[Verb("sync", HelpText = "Sync a library")]
public class SyncOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
    
    [Value(1, MetaName = "Library name or id", HelpText = "Name or Id of the library to be synced", Required = false)]
    public Guid? LibraryId { get; set; }

    [Option('a', "sync-all", Default = false, Required = false)]
    public bool SyncAll { get; set; } = false;
}