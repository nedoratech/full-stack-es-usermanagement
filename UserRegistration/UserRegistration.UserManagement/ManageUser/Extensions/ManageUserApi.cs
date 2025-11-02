using Asp.Versioning;
using Common.Contracts.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UserRegistration.WebApi.Contracts.Request;
using UserRegistration.WebApi.Contracts.Response;

namespace UserRegistration.UserManagement.ManageUser.Extensions;

public static class ManageUserApi
{
    private static readonly ApiVersion V1 = new(1, 0);

    public static IEndpointRouteBuilder MapManageUserApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v{version:apiVersion}/usermanagement/user", CrateUser)
            .MapToApiVersion(V1)
            .WithName("CrateUser");
        
        return app;
    }
    
    [EndpointSummary("Creates a new user")]
    [EndpointDescription("Creates a new user")]
    [ProducesResponseType(typeof(Response<CreateUserResponse>), StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound, "application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError, "application/json")]
    private static async Task<IResult> CrateUser([FromBody]CreateUserRequest request)
    {
        return Results.Ok(new
        {
            Usermane = "test"
        });
    }
}
