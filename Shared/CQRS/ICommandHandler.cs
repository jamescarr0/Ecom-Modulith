using MediatR;

namespace Shared.CQRS;

internal interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
{
}

internal interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}
