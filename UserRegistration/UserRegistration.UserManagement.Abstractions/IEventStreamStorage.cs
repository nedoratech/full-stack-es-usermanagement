namespace UserRegistration.UserManagement.Abstractions;

public interface IEventStreamStorage
{
    Task<IReadOnlyList<object>> FindByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken);
    Task AppendEventsAsync(Guid aggregateId, IEnumerable<object> events, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}