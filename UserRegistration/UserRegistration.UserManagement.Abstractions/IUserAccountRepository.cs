namespace UserRegistration.UserManagement.Abstractions;

public interface IUserAccountRepository
{
    Task<IUserAccount> CreateOrLoadAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
