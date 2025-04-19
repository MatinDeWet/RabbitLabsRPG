using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace BuildingBlock.Application.CQRS;

[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface for CQRS commands")]
public interface ICommand : ICommand<Unit>;

[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface for CQRS commands")]
public interface ICommand<out TResponse> : IRequest<TResponse>;
