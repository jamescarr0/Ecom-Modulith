﻿namespace Shared.DDD;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeuedEvents = domainEvents.ToArray();
        domainEvents.Clear();
        return dequeuedEvents;
    }
}
