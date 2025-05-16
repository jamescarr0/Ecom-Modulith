using MediatR;

namespace Shared.CQRS.Command;

public interface ICommand : ICommand<Unit>
{ }

public interface ICommand<out TResponse> : IRequest<TResponse>
{ }
