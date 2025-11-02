namespace UR.Events;

public sealed class AccountActivated
{
    public Guid AggregateId { get; set; }
    public DateTime OccurredAt { get; set; }
}

