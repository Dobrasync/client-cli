using System.Runtime.InteropServices;
using Lamashare.CLI.Const;
using Lamashare.CLI.Worker;
using Microsoft.Extensions.Hosting;

namespace Lamashare.CLI.Services.Command.Commands.Daemon;

public class DaemonCommand(ILoggerService logger) : ICommand
{
    public string GetName()
    {
        return "daemon";
    }

    public Task<int> Execute(string[] args)
    {
        if (OperatingSystem.IsLinux())
        {
            SetupDaemonLinux();
        }
        else
        {
            logger.LogFatal("Your operating system is not supported. Supported operating systems are: Linux");
        }

        return Task.FromResult(ExitCodes.Success);
    }

    private void SetupDaemonLinux()
    {
        Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<LamashareWorker>();
            })
            .UseSystemd();
    }
}