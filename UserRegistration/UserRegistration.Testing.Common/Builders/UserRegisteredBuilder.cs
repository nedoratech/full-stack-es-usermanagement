using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class UserRegisteredBuilder
{
    internal static UserRegistered Valid(Action<UserRegistered>? modify = null)
    {
        var @event = new UserRegistered
        {
            AggregateId = WellKnown.UserId,
            Name = "John Doe",
            Email = WellKnown.EmailAddress,
            Phone = "+1-555-123-4567",
            Address = "123 Main Street",
            OccurredAt = DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}

