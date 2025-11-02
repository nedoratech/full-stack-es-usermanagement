namespace UR.Events;

public sealed class EmailVerified
{
    public Guid AggregateId { get; set; }
    public DateTime OccurredAt { get; set; }
}

