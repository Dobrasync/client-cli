namespace Lamashare.CLI.Services.Command.Commands.Login;

[Verb("login", HelpText = "Login operation")]
public class LoginOptions : BaseCommandOptions
{
    [Value(0, MetaName = "Action", Required = true)]
    public string Action { get; set; }
}