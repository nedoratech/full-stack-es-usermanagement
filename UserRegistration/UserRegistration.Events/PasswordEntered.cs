namespace UR.Events;

public sealed class PasswordEntered
{
    public Guid AggregateId { get; set; }
    public string Password { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}