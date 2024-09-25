namespace Lamashare.CLI.Storage.Service;

public interface ILoggerService
{
    public void LogDebug(string msg);

    public void LogInfo(string msg);

    public void LogWarn(string msg);

    public void LogError(string msg);

    public void LogFatal(string msg);
}