using DDDWithEventSourcingBasedOnMarten.Domain;
using FluentAssertions;
using Xunit;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.Domain.Tests.ValueObjects
{
    public class ExpenseUnitPriceTests
    {

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void should_pass_validation_for_specific_unit_price(decimal value)
        {
            var unitPrice = new ExpenseUnitPrice(value);
            unitPrice.Value.Should().Be(value);
        }
    }
}
