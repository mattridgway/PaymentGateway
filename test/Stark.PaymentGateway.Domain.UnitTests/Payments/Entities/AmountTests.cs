using FluentAssertions;
using Stark.PaymentGateway.Domain.Payments.Entities;
using System;
using Xunit;

namespace Stark.PaymentGateway.Domain.UnitTests.Payments.Entities
{
    public class AmountTests
    {
        [Theory]
        [InlineData(-1, "GBP")]
        [InlineData(0, "GBP")]
        [InlineData(1, "")]
        [InlineData(1, " ")]
        [InlineData(1, null)]
        public void InvalidAmounts(decimal value, string currency)
        {
            Action act = () => { new Amount(value, currency); };
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(1, "GBP")]
        [InlineData(10, "GBP")]
        [InlineData(100, "GBP")]
        [InlineData(100, "USD")]
        [InlineData(100, "EUR")]
        public void ValidAmounts(decimal value, string currency)
        {
            Action act = () => { new Amount(value, currency); };
            act.Should().NotThrow<Exception>();
        }
    }
}
