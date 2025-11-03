namespace UserRegistration.UserManagement.Abstractions;

public interface IUserAccount
{
    Guid Id { get; }
    IUserAccount FromEvents(IEnumerable<object> events);
    void Append(object @event);
}
