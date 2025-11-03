using System.Reflection;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.UserManagement;

internal sealed class UserAccountRepository(
    IUserAccount userAccount, 
    IEventStreamStorage eventStreamStorage) : IUserAccountRepository
{
    async Task<IUserAccount> IUserAccountRepository.CreateOrLoadAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        var stream = await eventStreamStorage.FindByAggregateIdAsync(aggregateId, cancellationToken);
        return userAccount.FromEvents(stream);
    }

    async Task IUserAccountRepository.SaveChangesAsync(CancellationToken cancellationToken)
    {
        var userAccountType = userAccount.GetType();
        var pendingEventsProperty = userAccountType.GetProperty("PendingEvents", 
            BindingFlags.NonPublic | BindingFlags.Instance);

        if (pendingEventsProperty == null)
        {
            throw new InvalidOperationException("Could not find PendingEvents property on UserAccount.");
        }

        var pendingEvents = (IReadOnlyList<IEvent>?)pendingEventsProperty.GetValue(userAccount);
        
        if (pendingEvents == null || pendingEvents.Count == 0)
        {
            return;
        }
        
        await eventStreamStorage.AppendEventsAsync(userAccount.Id, pendingEvents, cancellationToken);
        await eventStreamStorage.SaveChangesAsync(cancellationToken);
    }
}
