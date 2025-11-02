using UR.Events.Abstractions;

namespace UR.Events;

public record UsernameEntered : IEvent
{
    public Guid AggregateId { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}

