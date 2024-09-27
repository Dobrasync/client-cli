namespace Lamashare.CLI.Services.Command.Commands.Clone;


[Verb("clone", HelpText = "Clone a library")]
public class CloneOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; }
    
    [Value(1, MetaName = "Library Id", HelpText = "Id of the library to be cloned", Required = true)]
    public Guid LibraryId { get; set; }
}