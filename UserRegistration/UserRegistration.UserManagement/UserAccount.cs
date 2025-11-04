using UR.Events;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.UserManagement;

internal sealed class UserAccount : IUserAccount
{
    private List<IEvent> _pendingEvents = new();

    private string _email = string.Empty;
    private string _phone = string.Empty;
    private string _address = string.Empty;
    private string _password = string.Empty;
    private bool _isEmailVerified;
    private bool _isActivated;
    private int _version;

    public Guid Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public IUserAccount FromEvents(IEnumerable<IEvent> events)
    {
        var user = new UserAccount();
        foreach (var @event in events)
        {
            user.Apply(@event);
        }
        return user;
    }

    public void Append(IEvent @event)
    {
        Apply(@event);
    }
    
    private void Apply(IEvent @event)
    {
        switch (@event)
        {
            case UserRegistrationRequested userRegistrationRequested:
                Apply(userRegistrationRequested);
                break;
            case UserRegistered userRegistered:
                Apply(userRegistered);
                break;
            case UsernameEntered usernameEntered:
                Apply(usernameEntered);
                break;
            case EmailEntered emailEntered:
                Apply(emailEntered);
                break;
            case PasswordEntered passwordEntered:
                Apply(passwordEntered);
                break;
            case PhoneEntered phoneEntered:
                Apply(phoneEntered);
                break;
            case AddressEntered addressEntered:
                Apply(addressEntered);
                break;
            case EmailVerified emailVerified:
                Apply(emailVerified);
                break;
            case AccountActivated accountActivated:
                Apply(accountActivated);
                break;
            case ProfileUpdated profileUpdated:
                Apply(profileUpdated);
                break;
        }
        _version++;
        _pendingEvents.Add(@event);
    }

    private void Apply(UserRegistrationRequested @event)
    {
        if (@event.UserId == Guid.Empty)
        {
            throw new InvalidOperationException("Invalid user id");
        }
        
        Id = @event.UserId;
    }

    private void Apply(UserRegistered @event)
    {
        if (Id != Guid.Empty)
        {
            throw new InvalidOperationException("User has already been registered.");
        }

        Id = @event.AggregateId;
        Username = @event.Name;
        _email = @event.Email;
        _phone = @event.Phone;
        _address = @event.Address;
    }

    private void Apply(UsernameEntered @event)
    {
        EnsureUserExists();
        Username = @event.Username;
    }

    private void Apply(EmailEntered @event)
    {
        EnsureUserExists();
        _email = @event.Email;
        _isEmailVerified = false;
    }

    private void Apply(PasswordEntered @event)
    {
        EnsureUserExists();
        _password = @event.Password;
    }

    private void Apply(PhoneEntered @event)
    {
        EnsureUserExists();
        _phone = @event.Phone;
    }

    private void Apply(AddressEntered @event)
    {
        EnsureUserExists();
        _address = @event.Address;
    }

    private void Apply(EmailVerified @event)
    {
        EnsureUserExists();
        _isEmailVerified = true;
    }

    private void Apply(AccountActivated @event)
    {
        EnsureUserExists();
        
        if (!_isEmailVerified)
        {
            throw new InvalidOperationException("Cannot activate account before email is verified.");
        }
        
        _isActivated = true;
    }

    private void Apply(ProfileUpdated @event)
    {
        EnsureUserExists();
        
        if (!string.IsNullOrWhiteSpace(@event.Name))
        {
            Username = @event.Name;
        }       
        
        if (!string.IsNullOrWhiteSpace(@event.Email))
        { 
            var emailChanged = _email != @event.Email;
            _email = @event.Email;
            if (emailChanged)
                _isEmailVerified = false;
        }
        
        if (!string.IsNullOrWhiteSpace(@event.Phone))
        {
            _phone = @event.Phone;
        }        
        
        if (!string.IsNullOrWhiteSpace(@event.Address))
        {
            _address = @event.Address;
        } 
    }

    private void EnsureUserExists()
    {
        if (Id == Guid.Empty)
        {
            throw new InvalidOperationException("User has not been registered yet.");
        }    
    }
    
    private bool CanActivate()
    {
        return _isEmailVerified && !_isActivated;
    }
    
    private bool CanVerifyEmail()
    {
        return !string.IsNullOrWhiteSpace(_email) && !_isEmailVerified;
    }
    
    private IReadOnlyList<IEvent> PendingEvents {
        get {
            var pendingEvents = _pendingEvents.ToList();
            _pendingEvents = [];
            return pendingEvents;
        }
    }
}
