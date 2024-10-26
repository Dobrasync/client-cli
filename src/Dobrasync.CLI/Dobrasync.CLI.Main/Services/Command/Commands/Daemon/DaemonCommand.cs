using System.Runtime.InteropServices;
using Lamashare.CLI.Const;
using Lamashare.CLI.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;

namespace Lamashare.CLI.Services.Command.Commands.Daemon;

public class DaemonCommand(ILoggerService logger) : ICommand
{
    public string GetName()
    {
        return "daemon";
    }

    public async Task<int> Execute(string[] args)
    {
        // This is just a stub class to have the command show up in help list.
        logger.LogFatal("Stub command run. This should never happen.");
        return await Task.FromResult(ExitCodes.Failure);
    }
}