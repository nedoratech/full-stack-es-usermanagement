using UserRegistration.UserManagement.Abstractions;

namespace UR.Events;

public sealed class UsernameEntered : IEvent
{
    public Guid AggregateId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}

