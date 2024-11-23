using Lamashare.CLI.ApiGen.Mainline;
using Lamashare.CLI.Const;
using Lamashare.CLI.Db.Entities;
using Lamashare.CLI.Db.Enums;
using Lamashare.CLI.Db.Repo;
using Lamashare.CLI.Services.Auth;
using Lamashare.CLI.Services.Block;
using Lamashare.CLI.Services.SystemSetting;
using Lamashare.CLI.Worker;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;


#region Bootstrap

#region CLI Params

var parser = new Parser(x =>
{
    x.IgnoreUnknownArguments = true;
});
var result = parser.ParseArguments<Options>(args);
if (result.Errors.Any())
{
    return ExitCodes.Failure;
}
#endregion

#endregion

#region Services
var services = new ServiceCollection();
#region Context
services.AddDbContext<LamashareContext>(x =>
{
    x.UseSqlite($"Data Source={Constants.AppSqliteFilePath}");
});
services.AddScoped<IRepoWrapper, RepoWrapper>();
#endregion
#region System settings
services.AddScoped<ISystemSettingService, SystemSettingService>();
#endregion
#region Logging
services.AddScoped<ILoggerService, LoggerService>();
services.AddLogging(x =>
{
    x.ClearProviders();
    x.SetMinimumLevel(result.Value.LogDebug ? LogLevel.Debug : LogLevel.Information);
    
    x.AddFilter("Microsoft", LogLevel.Warning);
    x.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
    x.AddFilter("System", LogLevel.Warning);

    x.AddConsole();
});
#endregion
#region SyncService
services.AddScoped<ISyncService, SyncService>();
#endregion
#region CommandService
services.AddScoped<ICommandService, CommandService>();
#endregion
services.AddScoped<IBlockService, BlockService>();
services.AddScoped<IAuthService, AuthService>();
#region OAPI
services.AddHttpClient<IApiClient, ApiClient>((client, serviceProvider) =>
{
    ISystemSettingService settings = serviceProvider.GetRequiredService<ISystemSettingService>();
    string baseUrl = settings.GetSettingValue(ESystemSetting.REMOTE_ADDRESS) ?? "default";
    string? token = settings.GetSettingValue(ESystemSetting.AUTH_TOKEN);
    
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    
    var apiClient = new ApiClient(baseUrl, client);
    return apiClient;
});
#endregion
#endregion


#region Daemon mode
if (result.Value.Action is not null && result.Value.Action.Equals("daemon", StringComparison.OrdinalIgnoreCase))
{
    var hostBuilder = Host.CreateDefaultBuilder()
        .UseSystemd()
        .ConfigureServices((hostContext, s) =>
        {
            s.AddHostedService<LamashareWorker>();
            foreach (var ser in services)
            {
                s.Add(ser);
            }
        });

    var host = hostBuilder.Build();
    await host.RunAsync();

    return ExitCodes.Success;
}
#endregion

#region Build services
var servicesProvider = services.BuildServiceProvider();
#endregion
#region DB Migrations
using(var scope = servicesProvider.CreateScope())
{
    string? containingDir = Path.GetDirectoryName(Constants.AppSqliteFilePath);
    if (containingDir is null)
    {
        return ExitCodes.Failure;
    }
    Directory.CreateDirectory(containingDir);
    
    var dbContext = scope.ServiceProvider.GetRequiredService<LamashareContext>();
    dbContext.Database.EnsureCreated();
}
#endregion

var commandService = servicesProvider.GetRequiredService<ICommandService>();
int exitCode = await commandService.Consume(args);
return exitCode;