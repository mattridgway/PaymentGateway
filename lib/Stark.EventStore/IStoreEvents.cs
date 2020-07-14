using Stark.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stark.EventStore
{
    public interface IStoreEvents
    {
        Task SaveAggregateEventsAsync(string aggregateId, IEnumerable<AggregateEvent> aggregateEvents);
    }
}
