using UserRegistration.UserManagement.Abstractions;

namespace UR.Events;

public sealed class AccountActivated : IEvent
{
    public Guid AggregateId { get; set; }
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}

