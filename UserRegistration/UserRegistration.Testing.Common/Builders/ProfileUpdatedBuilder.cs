using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class ProfileUpdatedBuilder
{
    internal static ProfileUpdated Valid(Action<ProfileUpdated>? modify = null)
    {
        var @event = new ProfileUpdated
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

