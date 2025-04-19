using Microsoft.Extensions.Logging;

namespace BuildingBlock.Application.Logging.Events;

public static class LoggingEventIds
{
    public static readonly EventId PerformanceWarning = new(1, nameof(PerformanceWarning));
}
