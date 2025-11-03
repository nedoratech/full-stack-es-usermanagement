using System.Collections.Concurrent;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.Testing.Common.Fakes;

internal sealed class FakeEventStreamStorage : IEventStreamStorage
{
    private readonly ConcurrentDictionary<Guid, List<object>> _eventStreams = new();

    public Task<IReadOnlyList<object>> FindByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        if (_eventStreams.TryGetValue(aggregateId, out var events))
        {
            return Task.FromResult<IReadOnlyList<object>>(events);
        }

        return Task.FromResult<IReadOnlyList<object>>(Array.Empty<object>());
    }

    public Task AppendEventsAsync(Guid aggregateId, IEnumerable<object> events, CancellationToken cancellationToken)
    {
        var stream = _eventStreams.GetOrAdd(aggregateId, _ => new List<object>());
        stream.AddRange(events);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public void AppendEvents(Guid aggregateId, params object[] events)
    {
        var stream = _eventStreams.GetOrAdd(aggregateId, _ => new List<object>());
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
