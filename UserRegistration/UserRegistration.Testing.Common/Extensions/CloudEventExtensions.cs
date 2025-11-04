using System.Reflection;
using UserRegistration.UserManagement.Abstractions;
using UserRegistration.WebApi.Contracts.Shared;

namespace UserRegistration.Testing.Common.Extensions;

internal static class CloudEventExtensions
{
    internal static CloudEvent<object> ToCloudEvent<T>(this T @event) where T : IEvent
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));

        var eventType = typeof(T);
        var aggregateId = GetAggregateId(@event);
        var occurredAt = GetOccurredAt(@event);

        return new CloudEvent<object>
        {
            Type = eventType.Name,
            Source = "user-registration",
            Subject = aggregateId.ToString(),
            Time = occurredAt,
            Data = @event
        };
    }

    private static Guid GetAggregateId<T>(T @event) where T : IEvent
    {
        var aggregateIdProperty = typeof(T).GetProperty("AggregateId", BindingFlags.Public | BindingFlags.Instance);
        if (aggregateIdProperty?.PropertyType == typeof(Guid))
        {
            return (Guid)(aggregateIdProperty.GetValue(@event) ?? Guid.Empty);
        }
        throw new InvalidOperationException($"Event type {typeof(T).Name} does not have an AggregateId property of type Guid");
    }

    private static DateTime GetOccurredAt<T>(T @event) where T : IEvent
    {
        var occurredAtProperty = typeof(T).GetProperty("OccurredAt", BindingFlags.Public | BindingFlags.Instance);
        if (occurredAtProperty?.PropertyType == typeof(DateTime))
        {
            return (DateTime)(occurredAtProperty.GetValue(@event) ?? DateTime.UtcNow);
        }

        return DateTime.UtcNow;
    }
}
