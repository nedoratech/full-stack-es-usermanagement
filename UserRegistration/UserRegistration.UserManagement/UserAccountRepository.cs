namespace UserRegistration.UserManagement;

internal sealed class UserAccountRepository(IUserAccount userAccount, IEventStreamStorage eventStreamStorage) : IUserAccountRepository
{
    async Task<IUserAccount> IUserAccountRepository.CreateOrLoadAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        var stream = await eventStreamStorage.FindByAggregateIdAsync(aggregateId, cancellationToken);
        return userAccount.FromEvents(stream);
    }

    async Task IUserAccountRepository.SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}