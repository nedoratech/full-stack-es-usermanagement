using UR.Events;
using UserRegistration.Testing.Common.Constants;

namespace UserRegistration.Testing.Common.Builders;

internal sealed class AccountActivatedBuilder
{
    internal static AccountActivated Valid(Action<AccountActivated>? modify = null)
    {
        var @event = new AccountActivated
        {
            AggregateId = WellKnown.UserId,
            OccurredAt = DateTime.UtcNow
        };
        
        modify?.Invoke(@event);
        return @event;
    }
}

