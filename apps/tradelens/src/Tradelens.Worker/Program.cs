using OpenTelemetry.Logs;
using Tradelens.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<TestWorker>();
builder.Services.AddHostedService<DbSyncWorker>();

builder.Logging.ClearProviders().AddOpenTelemetry(log => log.AddConsoleExporter());

var host = builder.Build();
host.Run();