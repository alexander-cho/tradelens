namespace Tradelens.Worker.Services;

public class TestWorker(ILogger<DbSyncWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("TestWorker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(100000, stoppingToken);
        }
    }
}