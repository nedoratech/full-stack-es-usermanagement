using UR.Events.Abstractions;

namespace UR.Events;

public record PhoneEntered : IEvent
{
    public Guid AggregateId { get; init; }
    public string Phone { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}

