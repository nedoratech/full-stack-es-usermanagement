using UR.Events.Abstractions;

namespace UR.Events;

public record EmailEntered : IEvent
{
    public Guid AggregateId { get; init; }
    public string Email { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}

