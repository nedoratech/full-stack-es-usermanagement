namespace UserRegistration.UserManagement.Abstractions;

public interface IUserAccount
{
    Guid Id { get; }
    IUserAccount FromEvents(IEnumerable<IEvent> events);
    void Append(IEvent @event);
}
