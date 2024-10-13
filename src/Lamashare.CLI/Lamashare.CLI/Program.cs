using Lamashare.CLI.ApiGen.Mainline;
using Lamashare.CLI.Const;
using Lamashare.CLI.Db.Repo;
using Lamashare.CLI.Services.Block;
using Lamashare.CLI.Services.SystemSetting;
using Lamashare.CLI.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;


#region Bootstrap

#region CLI Params

var parser = new Parser(x => x.IgnoreUnknownArguments = true);
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
services.AddSerilog(x =>
{
    x.WriteTo.Console();
    if (result.Value.LogDebug)
    {
        x.MinimumLevel.Debug();
        x.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Debug);
    }
    else
    {
        x.MinimumLevel.Information();
        x.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Error);
    }
});
services.AddScoped<ILoggerService, LoggerService>();
#endregion
#region SyncService
services.AddScoped<ISyncService, SyncService>();
#endregion
#region CommandService
services.AddScoped<ICommandService, CommandService>();
#endregion
services.AddScoped<IBlockService, BlockService>();
#region OAPI
services.AddHttpClient<IApiClient, ApiClient>(_ => new ApiClient("http://localhost:5127", new HttpClient()));
#endregion
#endregion


#region Daemon mode
if (result.Value.Action.Equals("daemon", StringComparison.OrdinalIgnoreCase))
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
    var dbContext = scope.ServiceProvider.GetRequiredService<LamashareContext>();
    dbContext.Database.EnsureCreated();
}
#endregion

var commandService = servicesProvider.GetRequiredService<ICommandService>();
int exitCode = await commandService.Consume(args);
return exitCode;