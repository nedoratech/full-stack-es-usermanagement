using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class EmailVerifiedBuilder
{
    internal static EmailVerified Valid(Action<EmailVerified>? modify = null)
    {
        var @event = new EmailVerified
        {
            AggregateId = WellKnown.UserId,
            OccurredAt = DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}

