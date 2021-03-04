using DDDWithEventSourcingBasedOnMarten.Domain;
using FluentAssertions;
using Xunit;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.Domain.Tests.ValueObjects
{
    public class ExpenseQuantityTests
    {

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void should_pass_validation_for_specific_quantity(decimal value)
        {
            var quantity = new ExpenseQuantity(value);
            quantity.Value.Should().Be(value);
        }
    }
}
