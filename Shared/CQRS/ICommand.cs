using MediatR;

namespace Shared.CQRS;

internal interface ICommand : ICommand<Unit>
{ }

internal interface ICommand<out TResponse> : IRequest<TResponse>
{ }
