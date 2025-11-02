using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class PhoneEnteredBuilder
{
    internal static PhoneEntered Valid(Action<PhoneEntered>? modify = null)
    {
        var @event = new PhoneEntered
        {
            AggregateId = WellKnown.UserId,
            Phone = "+1-555-123-4567",
            OccurredAt = DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}

