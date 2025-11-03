namespace UserRegistration.UserManagement.Abstractions;

public interface IEvent
{
    Guid AggregateId { get; }
    DateTime OccurredAt { get; }
    int Version { get; }
}

