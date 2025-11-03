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
    
    private IDbContextTransaction? _transaction;

    public async Task<IReadOnlyList<IEvent>> FindByAggregateIdAsync(
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
        IEnumerable<IEvent> events, 
        CancellationToken cancellationToken)
    {
        var eventList = events.ToList();
        if (eventList.Count == 0)
        {            
            return;
        }
        
        if (_transaction == null)
        {
            _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        }

        var entities = eventList.Select(@event => new EventStoreEntity
            {
                AggregateId = @event.AggregateId,
                Type = @event.GetType().FullName!,
                Body = JsonSerializer.Serialize(@event, _jsonOptions),
                Version = @event.Version,
                OccurredAt = @event.OccurredAt
            })
            .ToList();

        context.Events.AddRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    private IEvent DeserializeEvent(EventStoreEntity entity)
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

        if (deserialized is not IEvent @event)
        {
            throw new InvalidOperationException(
                $"Deserialized object of type '{entity.Type}' does not implement IEvent.");
        }

        return @event;
    }
}
