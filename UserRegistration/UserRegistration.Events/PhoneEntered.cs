namespace UR.Events;

public sealed class PhoneEntered
{
    public Guid AggregateId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}

