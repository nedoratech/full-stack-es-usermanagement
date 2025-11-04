using UserRegistration.UserManagement.Abstractions;
using UserRegistration.WebApi.Contracts.Shared;

namespace UserRegistration.WebApi.Contracts.Request;

public sealed class CreateUserRequest
{
    public Guid UserId { get; set; }
    public IReadOnlyList<CloudEvent<object>> Events { get; set; } =  new List<CloudEvent<object>>();
}