using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace UserRegistration.UserManagement.ManageUser.Extensions;

public static class ManageUserApi
{
    private static readonly ApiVersion V1 = new(1, 0);

    public static IEndpointRouteBuilder MapManageUserApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v{version:apiVersion}/user", GetUserAsync)
            .MapToApiVersion(V1)
            .WithName("GetUser");
        
        return app;
    }

    private static async Task<IResult> GetUserAsync()
    {
        return Results.Ok(new
        {
            Usermane = "test"
        });
    }
}
