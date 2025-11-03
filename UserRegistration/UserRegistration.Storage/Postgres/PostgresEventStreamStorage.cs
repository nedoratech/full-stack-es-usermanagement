using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UserRegistration.Storage.Postgres.Entities;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.Storage.Postgres;

internal sealed class PostgresEventStreamStorage(EventStoreDbContext context) : IEventStreamStorage
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    private IDbContextTransaction? _currentTransaction;

    public async Task<IReadOnlyList<object>> FindByAggregateIdAsync(
        Guid aggregateId, 
        CancellationToken cancellationToken)
    {
        var events = await context.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        return events.Select(DeserializeEvent).ToList();
    }

    public async Task AppendEventsAsync(
        Guid aggregateId, 
        IEnumerable<object> events, 
        CancellationToken cancellationToken)
    {
        var eventList = events.ToList();
        if (eventList.Count == 0)
            return;

        if (_currentTransaction == null)
        {
            _currentTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
        }
        
        var maxVersion = await context.Events
            .Where(e => e.AggregateId == aggregateId)
            .Select(e => (int?)e.Version)
            .MaxAsync(cancellationToken) ?? 0;

        var entities = new List<EventStoreEntity>();
        var version = maxVersion;

        foreach (var @event in eventList)
        {
            version++;
            entities.Add(new EventStoreEntity
            {
                AggregateId = aggregateId,
                Type = @event.GetType().FullName!,
                Body = JsonSerializer.Serialize(@event, _jsonOptions),
                Version = version,
                OccurredAt = GetOccurredAt(@event) ?? DateTime.UtcNow
            });
        }

        context.Events.AddRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        
        if (_currentTransaction != null)
        {
            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    private object DeserializeEvent(EventStoreEntity entity)
    {
        var eventType = Type.GetType(entity.Type);
        if (eventType == null)
        {
            throw new InvalidOperationException(
                $"Could not deserialize event. Type '{entity.Type}' not found.");
        }

        var deserialized = JsonSerializer.Deserialize(entity.Body, eventType, _jsonOptions);
        if (deserialized == null)
        {
            throw new InvalidOperationException(
                $"Could not deserialize event data for type '{entity.Type}'.");
        }

        return deserialized;
    }

    private static DateTime? GetOccurredAt(object @event)
    {
        var property = @event.GetType().GetProperty("OccurredAt");
        if (property?.PropertyType == typeof(DateTime))
        {
            return (DateTime?)property.GetValue(@event);
        }
        return null;
    }
}
