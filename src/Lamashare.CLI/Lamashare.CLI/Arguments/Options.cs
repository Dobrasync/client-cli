namespace Lamashare.CLI.Arguments;

public class Options
{
    [Option('v', "verbose", Required = false, HelpText = "Set log output to output debug messages.")]
    public bool LogDebug { get; set; }

    [Value(0, MetaName = "action",  HelpText = "Action to execute (clone, remove, login, logout, scan).", Required = true)]
    public string Action { get; set; } = default!;
}