namespace Lamashare.CLI.Services.Command.Commands.List;

[Verb("list", HelpText = "List all local libraries")]
public class ListOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
}