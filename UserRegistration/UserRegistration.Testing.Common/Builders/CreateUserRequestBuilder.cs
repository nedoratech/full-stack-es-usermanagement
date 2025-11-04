using UserRegistration.Testing.Common.Constants;
using UserRegistration.Testing.Common.Extensions;
using UserRegistration.WebApi.Contracts.Request;
using UserRegistration.WebApi.Contracts.Shared;

namespace UserRegistration.Testing.Common.Builders;

internal static class CreateUserRequestBuilder
{
    internal static CreateUserRequest ValidEventStream(DateTime? now = null)
    {
        now ??= DateTime.UtcNow;
        
        return new CreateUserRequest
        {
            UserId = WellKnown.UserId,
            Events = new List<CloudEvent<object>>
            {
                UserRegistrationRequestedBuilder.Valid(x =>
                {
                    x.OccurredAt = now.Value.AddSeconds(1);
                }).ToCloudEvent(),
                EmailEnteredBuilder.Valid(x =>
                {
                    x.OccurredAt = now.Value.AddSeconds(2);
                }).ToCloudEvent(),
                PasswordEnteredBuilder.Valid(x =>
                {
                    x.OccurredAt = now.Value.AddSeconds(3);
                }).ToCloudEvent(),
                UsernameEnteredBuilder.Valid(x =>
                {
                    x.OccurredAt = now.Value.AddSeconds(4);
                }).ToCloudEvent(),
            }
        };
    }
}
