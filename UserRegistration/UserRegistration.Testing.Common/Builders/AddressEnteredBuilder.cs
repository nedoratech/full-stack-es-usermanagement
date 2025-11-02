using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class AddressEnteredBuilder
{
    internal static AddressEntered Valid(Action<AddressEntered>? modify = null)
    {
        var @event = new AddressEntered
        {
            AggregateId = WellKnown.UserId,
            Address = "123 Main Street",
            OccurredAt = DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}

