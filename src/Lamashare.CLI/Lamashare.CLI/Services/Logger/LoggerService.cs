namespace Lamashare.CLI.Services.Logger;

public class LoggerService(ILogger logger) : ILoggerService
{
    public void LogDebug(string msg)
    {
        logger.Debug(msg);
    }
    
    public void LogInfo(string msg)
    {
        logger.Information(msg);
    }
    
    public void LogWarn(string msg)
    {
        logger.Warning(msg);
    }
    
    public void LogError(string msg)
    {
        logger.Error(msg);
    }

    public void LogFatal(string msg)
    {
        logger.Fatal(msg);
    }
}