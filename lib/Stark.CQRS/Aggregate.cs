using System;

namespace Stark.CQRS
{
    public abstract class Aggregate
    {
        public abstract Guid Id { get; }

        private const long _newAggregateVersions = -1;
        protected long Version { get; set; } = _newAggregateVersions;
    }
}
