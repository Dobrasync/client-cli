#region Bootstrap

#region CLI Params

using Lamashare.CLI.ApiGen.Mainline;

var result = Parser.Default.ParseArguments<Options>(args);

if (result.Errors.Any()) return 1;
#endregion
#endregion
#region Services
var services = new ServiceCollection();
#region Context
services.AddDbContext<LamashareContext>(x =>
{
    x.UseSqlite($"Data Source={Constants.AppSqliteFilePath}");
});
#endregion
#region Logging
services.AddSerilog(x =>
{
    x.WriteTo.Console();
});
services.AddScoped<ILoggerService, LoggerService>();
#endregion
#region SyncService
services.AddScoped<ISyncService, SyncService>();
#endregion
#region CommandService
services.AddScoped<ICommandService, CommandService>();
#endregion
#region OAPI
services.AddHttpClient<IApiClient, ApiClient>(_ => new ApiClient("http://localhost:5127", new HttpClient()));
#endregion
#endregion

var servicesProvider = services.BuildServiceProvider();
var commandService = servicesProvider.GetRequiredService<ICommandService>();
int exitCode = await commandService.Consume(args);
return exitCode;