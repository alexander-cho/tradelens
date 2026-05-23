namespace Tradelens.Worker.Services;

public class DbSyncWorker(ILogger<DbSyncWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("DbSyncWorker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}
