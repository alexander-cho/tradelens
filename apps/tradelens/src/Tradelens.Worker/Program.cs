using Tradelens.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DbSyncWorker>();

var host = builder.Build();
host.Run();