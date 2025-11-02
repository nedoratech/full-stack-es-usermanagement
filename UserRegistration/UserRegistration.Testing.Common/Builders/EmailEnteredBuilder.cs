using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class EmailEnteredBuilder
{
    internal static EmailEntered Valid(Action<EmailEntered>? modify = null)
    {
        var @event = new EmailEntered
        {
            AggregateId = WellKnown.UserId,
            Email = WellKnown.EmailAddress,
            OccurredAt =  DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}