namespace Lamashare.CLI.Arguments;

public class Options
{
    [Option('v', "verbose", Required = false, HelpText = "Set log output to output debug messages.")]
    public bool LogDebug { get; set; }

    [Value(0, MetaName = "action",  HelpText = "Action", Required = false)]
    public string? Action { get; set; } = default!;
}