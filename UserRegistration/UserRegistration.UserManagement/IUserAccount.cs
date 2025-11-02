namespace UserRegistration.UserManagement;

internal interface IUserAccount
{
    UserAccount FromEvents(IEnumerable<object> events);
    void Append(object @event);
}