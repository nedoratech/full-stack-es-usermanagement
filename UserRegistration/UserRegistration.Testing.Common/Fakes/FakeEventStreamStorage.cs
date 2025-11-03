using System.Collections.Concurrent;
using UR.Events;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.Testing.Common.Fakes;

internal sealed class FakeEventStreamStorage : IEventStreamStorage
{
    private readonly ConcurrentDictionary<Guid, List<IEvent>> _eventStreams = new();

    public Task<IReadOnlyList<IEvent>> FindByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        if (_eventStreams.TryGetValue(aggregateId, out var events))
        {
            return Task.FromResult<IReadOnlyList<IEvent>>(events);
        }

        return Task.FromResult<IReadOnlyList<IEvent>>(Array.Empty<IEvent>());
    }

    public Task AppendEventsAsync(Guid aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken)
    {
        var stream = _eventStreams.GetOrAdd(aggregateId, _ => new List<IEvent>());
        stream.AddRange(events);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
    
    public void AppendEvents(Guid aggregateId, params IEvent[] events)
    {
        var stream = _eventStreams.GetOrAdd(aggregateId, _ => new List<IEvent>());
        stream.AddRange(events);
    }
    
    public void Clear()
    {
        _eventStreams.Clear();
    }
    
    public int GetEventCount(Guid aggregateId)
    {
        return _eventStreams.TryGetValue(aggregateId, out var events) ? events.Count : 0;
    }
}
