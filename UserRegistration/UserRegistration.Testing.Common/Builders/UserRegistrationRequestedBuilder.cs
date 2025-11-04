using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal static class UserRegistrationRequestedBuilder
{
    internal static UserRegistrationRequested Valid(Action<UserRegistrationRequested>? modify = null)
    {
        var @event = new UserRegistrationRequested
        {
            UserId = WellKnown.UserId,
            OccurredAt = DateTime.UtcNow,
            Version = 0
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}