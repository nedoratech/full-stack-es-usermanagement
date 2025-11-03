namespace UserRegistration.UserManagement.Abstractions;

public interface IEvent
{
    Guid AggregateId { get; set; }
    DateTime OccurredAt { get; set; }
    int Version { get; set; }
}

