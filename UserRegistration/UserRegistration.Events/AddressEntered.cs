using UR.Events.Abstractions;

namespace UR.Events;

public record AddressEntered : IEvent
{
    public Guid AggregateId { get; init; }
    public string Address { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}

