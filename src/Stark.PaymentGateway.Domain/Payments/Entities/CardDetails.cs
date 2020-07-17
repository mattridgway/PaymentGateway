using Stark.CQRS;
using System;
using System.Collections.Generic;

namespace Stark.PaymentGateway.Domain.Payments.Entities
{
    public class CardDetails : ValueObject
    {
        public string CardNumber { get; }
        public int ExpMonth { get; }
        public int ExpYear { get; }

        public CardDetails(string cardNumber, int? expMonth, int? expYear)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                throw new ArgumentNullException(nameof(cardNumber));

            //TODO: Validate CardNumber is valid (corrcet number of digits, in the correct ranges, etc)

            if (!expMonth.HasValue || expMonth.Value < 1 || expMonth.Value > 12)
                throw new ArgumentException($"Must have a value between 1 and 12 (inclusive).", nameof(expMonth));

            if (!expYear.HasValue || expYear.Value < 0 || expYear.Value > 99)
                throw new ArgumentException($"Must have a value between 0 and 99 (inclusive).", nameof(expYear));

            //TODO: Validate expiry are numbers and in the future

            CardNumber = cardNumber;
            ExpMonth = expMonth.Value;
            ExpYear = expYear.Value;
        }

        protected override IEnumerable<object> GetEqualityProperties()
        {
            yield return CardNumber;
            yield return ExpMonth;
            yield return ExpYear;
        }
    }
}
