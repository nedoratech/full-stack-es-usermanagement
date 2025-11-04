using UserRegistration.UserManagement.Abstractions;

namespace UR.Events;

public sealed class UserRegistrationRequested : IEvent
{
    public Guid AggregateId { get; set; }
    public Guid UserId { get; set; }
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}