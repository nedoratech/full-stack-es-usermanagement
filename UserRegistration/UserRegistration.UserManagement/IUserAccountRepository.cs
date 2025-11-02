namespace UserRegistration.UserManagement;

internal interface IUserAccountRepository
{
    Task<IUserAccount> CreateOrLoadAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}