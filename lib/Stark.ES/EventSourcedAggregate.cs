using Stark.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stark.ES
{
    public abstract class EventSourcedAggregate : Aggregate
    {
        private readonly ICollection<AggregateEvent> _uncommittedEvents = new LinkedList<AggregateEvent>();
        public IEnumerable<AggregateEvent> UncommittedEvents() => _uncommittedEvents;
        public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

        protected void ApplyEvents(IEnumerable<AggregateEvent> events)
        {
            if (events is null || !events.Any())
                throw new ArgumentException($"{nameof(events)} cannot be null or empty");

            foreach (var e in events)
                ApplyEvent(e);
        }

        private void ApplyEvent(AggregateEvent @event)
        {
            ((dynamic)this).Apply((dynamic)@event);
            Version = @event.Version;
        }

        protected void RaiseEvent(AggregateEvent @event)
        {
            if (@event is null)
                throw new ArgumentException($"{nameof(@event)} cannot be null");

            @event.Version = Version + 1;
            ApplyEvent(@event);
            _uncommittedEvents.Add(@event);
        }
    }
}
