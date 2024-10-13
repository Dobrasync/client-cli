using System.Diagnostics;
using Lamashare.CLI.Const;

namespace Lamashare.CLI.Services.Command.Commands.Autostart;

public class AutostartCommand(ILoggerService logger) : ICommand
{
    public string GetName()
    {
        return "autostart";
    }

    public Task<int> Execute(string[] args)
    {
        var result = Parser.Default.ParseArguments<AutostartOptions>(args);
        if (result.Errors.Any()) return Task.FromResult(ExitCodes.Failure);

        if (result.Value.Disable)
        {
            return Task.FromResult(Disable());
        }
        
        if (result.Value.Enable)
        {
            if (!DoesServiceConfigExist())
            {
                Setup();
            }

            return Task.FromResult(Enable());
        }
        
        logger.LogFatal("No parameter provided.");
        return Task.FromResult(ExitCodes.Failure);
    }

    private bool DoesServiceConfigExist()
    {
        string file = GetServicePath();
        return File.Exists(file);
    }

    private string GetServicePath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config/systemd/user/",
            "lamashare.service"
        );
    }
    
    private int Setup()
    {
        logger.LogInfo("Setting up service config...");
        string? execFile = System.Reflection.Assembly.GetEntryAssembly()?.Location;
        if (execFile == null)
        {
            logger.LogFatal("Could not determine executable file location.");
            return ExitCodes.Failure;
        }
        string? execDir = Path.GetDirectoryName(execFile);
        if (execDir == null)
        {
            logger.LogFatal("Could not determine executable directory location.");
            return ExitCodes.Failure;
        }
        execFile = execFile.Replace(".dll", "");
        
        string txt = File.ReadAllText(Path.Combine(execDir, "Resources/lamashare.service"));
        
        string servicePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config/systemd/user/",
            "lamashare.service"
        );

        string? parent = Path.GetDirectoryName(servicePath);
        if (parent == null)
        {
            logger.LogFatal("Service parent dir does not exist, can set up.");
            return ExitCodes.Failure;
        }
        Directory.CreateDirectory(parent);
        File.WriteAllText(servicePath, txt.Replace("{ExecStart}", $"\"{execFile}\" daemon"));
        logger.LogInfo($"Service config set up ({servicePath}).");
        logger.LogInfo("Reloading systemd configuration...");
        Process.Start("systemctl", "--user daemon-reload").WaitForExit();
        logger.LogInfo("Systemd configuration reloaded.");
        return ExitCodes.Success;
    }
    
    private int Enable()
    {
        Process.Start("systemctl", "--user enable lamashare").WaitForExit();
        return ExitCodes.Success;
    }
    
    private int Disable()
    {
        Process.Start("systemctl", "--user disable lamashare").WaitForExit();
        return ExitCodes.Success;
    }
}