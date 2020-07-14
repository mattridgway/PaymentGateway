using Stark.CQRS;
using System;
using System.Collections.Generic;

namespace Stark.PaymentGateway.Domain.Payments.Entities
{
    public class Amount : ValueObject
    {
        public decimal Value { get; }
        public string Currency { get; }

        public Amount(decimal? value, string currency)
        {
            if (!value.HasValue)
                throw new ArgumentNullException(nameof(value));

            if (value <= 0)
                throw new ArgumentException($"Must be a positive number.", nameof(value));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentNullException(nameof(currency));

            Value = value.Value;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityProperties()
        {
            yield return Value;
            yield return Currency;
        }
    }
}
