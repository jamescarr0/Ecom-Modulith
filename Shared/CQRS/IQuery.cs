using MediatR;

namespace Shared.CQRS;

internal interface IQuery<out T> : IRequest<T> where T : notnull
{
}
