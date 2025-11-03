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

        var pendingEvents = (IReadOnlyList<object>?)pendingEventsProperty.GetValue(userAccount);
        
        if (pendingEvents == null || pendingEvents.Count == 0)
        {
            return;
        }        
        
        // Append events to the transaction (but don't commit yet)
        await eventStreamStorage.AppendEventsAsync(userAccount.Id, pendingEvents, cancellationToken);
        
        // Commit the transaction
        await eventStreamStorage.SaveChangesAsync(cancellationToken);
    }
}
