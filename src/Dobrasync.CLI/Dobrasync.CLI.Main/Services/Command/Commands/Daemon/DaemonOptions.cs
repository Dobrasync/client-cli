namespace Lamashare.CLI.Services.Command.Commands.Sync;


[Verb("daemon", HelpText = "Launches the program as daemon to provide background services like library watcher for automatic sync.")]
public class DaemonOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
}