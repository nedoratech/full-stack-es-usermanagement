using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class PasswordEnteredBuilder
{
    internal static PasswordEntered Valid(Action<PasswordEntered>? modify = null)
    {
        var @event = new PasswordEntered
        {
            AggregateId = WellKnown.UserId,
            Password = "password#1!",
            OccurredAt = DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}