using UserRegistration.UserManagement.Abstractions;

namespace UR.Events;

public sealed class PhoneEntered : IEvent
{
    public Guid AggregateId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}

