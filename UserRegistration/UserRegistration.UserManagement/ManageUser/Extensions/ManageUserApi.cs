using System.Reflection;
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

internal sealed class ValidationErrorContext(string message) : ErrorContext("ValidationError")
{
    public string Message => message;
}

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
        try
        {
            // Load the Events assembly
            var validEventTypes = EventsAssemblyReference.Instance
                .GetTypes()
                .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToHashSet();

            // Extract events from CloudEvent wrappers and validate
            var events = new List<IEvent>();
            foreach (var cloudEvent in request.Events)
            {
                if (cloudEvent.Data == null)
                {
                    return Results.BadRequest(Response.WithError(
                        new ValidationErrorContext("Event data cannot be null").AddContext("message", "Event data cannot be null")));
                }

                var eventType = cloudEvent.Data.GetType();
                
                // Validate that the event is from the Events assembly
                if (!validEventTypes.Contains(eventType))
                {
                    return Results.BadRequest(Response.WithError(
                        new ValidationErrorContext($"Event type '{eventType.FullName}' is not a valid event from the Events assembly")
                            .AddContext("message", $"Event type '{eventType.FullName}' is not a valid event from the Events assembly")));
                }

                // Set AggregateId to UserId as a safety measure using reflection
                var aggregateIdProperty = eventType.GetProperty("AggregateId");
                if (aggregateIdProperty == null)
                {
                    return Results.BadRequest(Response.WithError(
                        new ValidationErrorContext($"Event type '{eventType.FullName}' does not have an AggregateId property")
                            .AddContext("message", $"Event type '{eventType.FullName}' does not have an AggregateId property")));
                }
                
                aggregateIdProperty.SetValue(cloudEvent.Data, request.UserId);
                
                events.Add(cloudEvent.Data);
            }

            if (events.Count == 0)
            {
                return Results.BadRequest(Response.WithError(
                    new ValidationErrorContext("At least one event is required")
                        .AddContext("message", "At least one event is required")));
            }

            // Load or create the user account
            var userAccount = await userAccountRepository.CreateOrLoadAsync(request.UserId, cancellationToken);
            
            // Append events to the user account
            foreach (var @event in events)
            {
                userAccount.Append(@event);
            }
            
            // Save changes (persists pending events)
            await userAccountRepository.SaveChangesAsync(cancellationToken);

            return Results.Ok(Response<CreateUserResponse>.WithResult(new CreateUserResponse
            {
                Username = "User created successfully"
            }));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
