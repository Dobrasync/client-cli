namespace Lamashare.CLI.Services.Command.Commands.Sync;


[Verb("scan", HelpText = "Scan a library for changes")]
public class ScanOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
    
    [Value(1, MetaName = "Library name or id", HelpText = "Name or Id of the library to be synced", Required = false)]
    public Guid? LibraryId { get; set; }

    [Option('a', "sync-all", Default = false, Required = false)]
    public bool SyncAll { get; set; } = false;
}