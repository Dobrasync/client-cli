#region Bootstrap

using CommandLine;
using Lamashare.CLI.Db;
using Lamashare.CLI.Services.Command;
using Lamashare.CLI.Storage.Arguments;
using Lamashare.CLI.Storage.Service;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

#region CLI Params
var result = Parser.Default.ParseArguments<Options>(args);

if (result.Errors.Any()) return 1;
#endregion
#endregion
#region Services
var services = new ServiceCollection();
#region Context
services.AddDbContext<LamashareContext>();
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
#endregion

var servicesProvider = services.BuildServiceProvider();
var commandService = servicesProvider.GetRequiredService<ICommandService>();
commandService.Consume(result);
return 0;