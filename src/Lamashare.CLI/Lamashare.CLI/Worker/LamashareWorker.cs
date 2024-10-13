using Lamashare.CLI.Const;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;

namespace Lamashare.CLI.Worker;

public class LamashareWorker(IServiceProvider serviceProvider) : BackgroundService
{
    private const int Delay = 60*1000;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerService>();
            var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
        
            if (!SystemdHelpers.IsSystemdService())
            {
                logger.LogFatal("Daemon mode can only be run by systemd.");
                return;
            }

            logger.LogInfo($"Worker running at: {DateTime.Now}");
            await syncService.SyncAllLibraries();
            await Task.Delay(Delay, stoppingToken);
        }
    }
}