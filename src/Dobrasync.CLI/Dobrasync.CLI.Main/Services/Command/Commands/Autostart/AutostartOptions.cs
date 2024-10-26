namespace Lamashare.CLI.Services.Command.Commands.Autostart;


[Verb("autostart", HelpText = "Adjust autostart behaviour.")]
public class AutostartOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; } = default!;
    
    [Option('d', "disable", HelpText = "Disables autostart on user login.", Default = false, Required = false)]
    public bool Disable { get; set; } = false;
    
    [Option('e', "enable", HelpText = "Enables autostart on user login.", Default = false, Required = false)]
    public bool Enable { get; set; } = false;
}