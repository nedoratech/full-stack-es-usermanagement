namespace UR.Events;

public sealed class UserRegistered
{
    public Guid AggregateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}

