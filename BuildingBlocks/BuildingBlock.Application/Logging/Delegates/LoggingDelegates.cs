using BuildingBlock.Application.Logging.Events;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.Application.Logging.Delegates;

public static class LoggingDelegates
{
    public static readonly Action<ILogger, string, int, object, Exception?> LogPerformanceWarning =
        LoggerMessage.Define<string, int, object>(
            LogLevel.Warning,
            LoggingEventIds.PerformanceWarning,
            "[PERFORMANCE] The request {Request} took {TimeTaken} seconds. with data = {RequestData}");
}
