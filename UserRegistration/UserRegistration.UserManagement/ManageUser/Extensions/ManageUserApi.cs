using System.Reflection;
using System.Text.Json;
using Asp.Versioning;
using Common.Contracts.Http;
using Common.Contracts.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UR.Events;
using UserRegistration.UserManagement.Abstractions;
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
    private static async Task<IResult> CrateUser(
        [FromBody] CreateUserRequest request,
        IUserAccountRepository userAccountRepository,
        CancellationToken cancellationToken)
    {
        if (request.Events.Any(x => x.Data is null))
        {
            var responseError = new ErrorContext(ErrorType.ValidationError);
            responseError.AddContext(ErrorCodes.Keys.CloudEvent, ErrorCodes.Codes.Invalid);
            return Results.Ok(Response.WithError(responseError));
        }
        
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        try
        {
            var events = new List<IEvent>();
            foreach (var cloudEvent in request.Events.OrderBy(x => x.Time))
            {
                var fullyQualifiedTypeName = $"{EventsAssemblyReference.Namespace}.{cloudEvent.Type}";
                var eventType = EventsAssemblyReference.Instance.GetType(fullyQualifiedTypeName);
                if (eventType == null)
                {
                    continue;
                }

                JsonElement body;
                if (cloudEvent.Data is JsonElement jsonBody)
                {
                    body = jsonBody;
                }
                else
                {
                    body = JsonSerializer.SerializeToElement(cloudEvent.Data, jsonOptions);
                }
                
                var deserializedEvent = body.Deserialize(eventType, jsonOptions);
                if (deserializedEvent is not IEvent @event)
                {
                    continue;
                }

                var aggregateIdProperty = eventType.GetProperty("AggregateId");
                if (aggregateIdProperty == null)
                {
                    continue;
                }
                
                aggregateIdProperty.SetValue(@event, request.UserId);
                
                events.Add(@event);
            }

            if (events.Count == 0)
            {
                var responseError = new ErrorContext(ErrorType.ValidationError);
                responseError.AddContext(ErrorCodes.Keys.Aggregate, ErrorCodes.Codes.Invalid);
                return Results.Ok(Response.WithError(responseError));
            }

            var userAccount = await userAccountRepository.CreateOrLoadAsync(request.UserId, cancellationToken);
            foreach (var @event in events)
            {
                userAccount.Append(@event);
            }
            
            await userAccountRepository.SaveChangesAsync(cancellationToken);

            var response = new CreateUserResponse
            {
                Username = userAccount.Username
            };

            return Results.Ok(Response<CreateUserResponse>.WithResult(response));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
