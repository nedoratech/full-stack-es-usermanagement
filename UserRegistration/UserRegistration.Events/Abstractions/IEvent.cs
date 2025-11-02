namespace UR.Events.Abstractions;

public interface IEvent
{
    Guid AggregateId { get; }
    DateTime OccurredAt { get; }
}

