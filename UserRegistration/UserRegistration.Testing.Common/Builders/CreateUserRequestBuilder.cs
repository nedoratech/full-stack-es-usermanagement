using UserRegistration.Testing.Common.Extensions;
using UserRegistration.UserManagement.Abstractions;
using UserRegistration.WebApi.Contracts.Request;
using UserRegistration.WebApi.Contracts.Shared;

namespace UserRegistration.Testing.Common.Builders;

internal static class CreateUserRequestBuilder
{
    internal static CreateUserRequest ValidEventStream()
    {
        return new CreateUserRequest
        {
            Events = new List<CloudEvent<IEvent>>
            {
                EmailEnteredBuilder.Valid().ToCloudEvent(),
                PasswordEnteredBuilder.Valid().ToCloudEvent(),
                UsernameEnteredBuilder.Valid().ToCloudEvent(),
            }
        };
    }
}
