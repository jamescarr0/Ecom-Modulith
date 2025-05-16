﻿using MediatR;

namespace Shared.CQRS.Query;

public interface IQuery<out T> : IRequest<T> where T : notnull
{
}
