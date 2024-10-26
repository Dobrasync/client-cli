namespace Lamashare.CLI.Services.Command.Commands.Clone;


[Verb("remove", HelpText = "Remove a library")]
public class RemoveOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
    
    [Value(1, MetaName = "Library name or id", HelpText = "Name or Id of the library to be removed", Required = true)]
    public Guid LibraryId { get; set; }

    [Option('r', "remove-directory", Default = false, Required = false)]
    public bool RemoveDirectory { get; set; } = false;
}