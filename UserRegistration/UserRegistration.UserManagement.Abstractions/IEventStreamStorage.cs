namespace UserRegistration.UserManagement.Abstractions;

public interface IEventStreamStorage
{
    Task<IReadOnlyList<IEvent>> FindByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken);
    Task AppendEventsAsync(Guid aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
