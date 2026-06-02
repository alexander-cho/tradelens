using OpenTelemetry.Logs;

namespace Tradelens.Api.Extensions;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddOpenTelemetry(log => log.AddConsoleExporter());

        return logging;
    }
}