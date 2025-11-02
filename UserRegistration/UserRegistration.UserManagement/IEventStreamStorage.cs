namespace UserRegistration.UserManagement;

public interface IEventStreamStorage
{
    Task<IReadOnlyList<object>> FindByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken);
}