using UserRegistration.UserManagement.Abstractions;

namespace UR.Events;

public sealed class EmailEntered : IEvent
{
    public Guid AggregateId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}

