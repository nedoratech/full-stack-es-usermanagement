using UR.Events.Abstractions;

namespace UR.Events;

public record ProfileUpdated : IEvent
{
    public Guid AggregateId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; }  = string.Empty;
    public string Address { get; init; }  = string.Empty;
    public DateTime OccurredAt { get; init; }
}

