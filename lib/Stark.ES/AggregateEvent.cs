using System;

namespace Stark.ES
{
    public abstract class AggregateEvent
    {
        public long Version { get; set; } // TODO: Can I make this immutable and not have a public setter

        public Guid SourceId { get; }

        protected AggregateEvent(Guid sourceId)
        {
            SourceId = sourceId;
        }
    }
}
