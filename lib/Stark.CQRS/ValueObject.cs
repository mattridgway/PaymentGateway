using System;
using System.Collections.Generic;
using System.Linq;

namespace Stark.CQRS
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityProperties();

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            var valueobject = (ValueObject)obj;

            return GetEqualityProperties()
                .SequenceEqual(valueobject.GetEqualityProperties());
        }

        public override int GetHashCode()
        {
            return GetEqualityProperties()
                .Select(prop => prop != null ? prop.GetHashCode() : 0)
                .Aggregate((left, right) => left ^ right);
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
