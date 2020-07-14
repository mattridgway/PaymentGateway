using System;

namespace Stark.CQRS
{
    public abstract class Command
    {
        public Guid? Id { get; }

        protected Command(Guid? id)
        {
            Id = id;
        }
    }
}
