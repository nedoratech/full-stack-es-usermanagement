using UserRegistration.WebApi.Contracts.Shared;

namespace UserRegistration.WebApi.Contracts.Request;

public sealed class CreateUserRequest
{
    public IReadOnlyList<CloudEvent<object>> Events { get; set; } =  new List<CloudEvent<object>>();
}