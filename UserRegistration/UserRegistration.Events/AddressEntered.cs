using UserRegistration.UserManagement.Abstractions;

namespace UR.Events;

public sealed class AddressEntered : IEvent
{
    public Guid AggregateId { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}

