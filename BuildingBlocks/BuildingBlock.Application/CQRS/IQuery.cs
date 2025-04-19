using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace BuildingBlock.Application.CQRS;

[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface for CQRS queries")]
public interface IQuery<out TResponse>
        : IRequest<TResponse>
        where TResponse : notnull;
