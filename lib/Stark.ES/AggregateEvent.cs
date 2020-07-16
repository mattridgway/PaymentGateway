using System;

namespace Stark.ES
{
    public abstract class AggregateEvent
    {
        public long Version { get; set; } // TODO: Can I make this immutable and not have a public setter

        public Guid AggregateId { get; }

        protected AggregateEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
