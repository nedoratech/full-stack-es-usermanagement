namespace UR.Events;

public sealed class EmailEntered
{
    public Guid AggregateId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}

