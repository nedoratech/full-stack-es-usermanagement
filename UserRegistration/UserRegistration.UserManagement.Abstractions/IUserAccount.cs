namespace UserRegistration.UserManagement.Abstractions;

public interface IUserAccount
{
    Guid Id { get; }
    string Username { get; }
    IUserAccount FromEvents(IEnumerable<IEvent> events);
    void Append(IEvent @event);
}
