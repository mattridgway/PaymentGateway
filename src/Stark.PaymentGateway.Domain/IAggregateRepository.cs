using Stark.ES;
using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Domain
{
    public interface IAggregateRepository<T> where T : EventSourcedAggregate
    {
        Task<T> GetAggregateAsync(Guid id);
        Task SaveAggregateAsync(EventSourcedAggregate aggregate);
    }
}
