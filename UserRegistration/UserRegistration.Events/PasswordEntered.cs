using UserRegistration.UserManagement.Abstractions;

namespace UR.Events;

public sealed class PasswordEntered : IEvent
{
    public Guid AggregateId { get; set; }
    public string Password { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}
