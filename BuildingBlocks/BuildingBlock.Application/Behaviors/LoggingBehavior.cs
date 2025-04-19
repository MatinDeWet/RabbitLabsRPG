using System.Diagnostics;
using BuildingBlock.Application.Logging.Delegates;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
                                (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
                                : IPipelineBehavior<TRequest, TResponse>
                                where TRequest : notnull, IRequest<TResponse>
                                where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(next, nameof(next));

        var timer = new Stopwatch();
        timer.Start();

        TResponse? response = await next(cancellationToken);

        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;

        if (timeTaken.Seconds > 3)
        {
            LoggingDelegates.LogPerformanceWarning(
                logger,
                typeof(TRequest).Name,
                timeTaken.Seconds,
                request,
                null
            );
        }

        return response;
    }
}
