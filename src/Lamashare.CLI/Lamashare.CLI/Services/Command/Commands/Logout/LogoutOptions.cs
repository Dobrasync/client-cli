namespace Lamashare.CLI.Services.Command.Commands.Login;

[Verb("logout", HelpText = "Logout operation")]
public class LogoutOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; }
}