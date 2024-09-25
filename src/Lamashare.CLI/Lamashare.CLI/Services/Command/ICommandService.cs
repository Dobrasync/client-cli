using CommandLine;

namespace Lamashare.CLI.Services.Command;

public interface ICommandService
{
    public void Consume(ParserResult<Lamashare.CLI.Storage.Arguments.Options> options);
}