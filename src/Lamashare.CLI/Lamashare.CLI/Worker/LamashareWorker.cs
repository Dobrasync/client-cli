using Microsoft.Extensions.Hosting;

namespace Lamashare.CLI.Worker;

public class LamashareWorker(ILoggerService logger) : BackgroundService
{
    private const int Delay = 10*1000;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInfo($"Worker launched at {DateTime.Now} with delay {Delay}.");
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInfo($"Worker running at: {DateTime.Now}");
            await Task.Delay(Delay, stoppingToken);
        }
    }
}