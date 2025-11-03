namespace UserRegistration.Storage.Postgres.Entities;

internal sealed class EventStoreEntity
{
    public long Id { get; set; }
    public Guid AggregateId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTime OccurredAt { get; set; }
}