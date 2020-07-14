using Stark.ES;
using Stark.EventStore;
using Stark.PaymentGateway.Domain;
using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.Repositories
{
    internal class EventSourcedAggregateRepository<T> : IAggregateRepository<T> where T : EventSourcedAggregate
    {
        private readonly IStoreEvents _eventStore;

        public EventSourcedAggregateRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<T> GetAggregateAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAggregateAsync(EventSourcedAggregate aggregate)
        {
            await _eventStore.SaveAggregateEventsAsync(aggregate.Id.ToString(), aggregate.UncommittedEvents());
            aggregate.ClearUncommittedEvents();
        }
    }
}
