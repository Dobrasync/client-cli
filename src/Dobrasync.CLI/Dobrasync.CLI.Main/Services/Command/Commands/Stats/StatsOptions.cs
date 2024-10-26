namespace Lamashare.CLI.Services.Command.Commands.Sync;


[Verb("stats", HelpText = "Lists some stats about the app")]
public class StatsOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
}