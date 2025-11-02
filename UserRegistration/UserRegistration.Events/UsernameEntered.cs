namespace UR.Events;

public sealed class UsernameEntered
{
    public Guid AggregateId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}

