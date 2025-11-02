using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class UsernameEnteredBuilder
{
    internal static UsernameEntered Valid(Action<UsernameEntered>? modify = null)
    {
        var @event = new UsernameEntered
        {
            AggregateId = WellKnown.UserId,
            Username = "john_doe",
            OccurredAt = DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}

