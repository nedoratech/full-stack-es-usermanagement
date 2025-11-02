using UR.Events.Abstractions;

namespace UR.Events;

public record AccountActivated : IEvent
{
    public Guid AggregateId { get; init; }
    public DateTime OccurredAt { get; init; }
}

