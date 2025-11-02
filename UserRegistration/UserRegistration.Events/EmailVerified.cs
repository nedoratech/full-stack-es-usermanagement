using UR.Events.Abstractions;

namespace UR.Events;

public record EmailVerified : IEvent
{
    public Guid AggregateId { get; init; }
    public DateTime OccurredAt { get; init; }
}

