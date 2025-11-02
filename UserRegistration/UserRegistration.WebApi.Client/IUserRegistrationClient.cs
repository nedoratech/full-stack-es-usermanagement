using Common.Contracts.Http;
using Refit;
using UserRegistration.WebApi.Contracts.Request;
using UserRegistration.WebApi.Contracts.Response;

namespace UserRegistration.WebApi.Client;

public interface IUserRegistrationClient
{
    [Post("/api/v{version}/usermanagement/user")]
    Task<Response<CreateUserResponse>> CrateUser([Body]CreateUserRequest request,
        [AliasAs("version")] string version = "1.0", 
        CancellationToken cancellationToken = default);
}