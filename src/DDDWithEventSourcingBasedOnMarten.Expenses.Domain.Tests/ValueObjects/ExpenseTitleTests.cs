using DDDWithEventSourcingBasedOnMarten.Domain;
using FluentAssertions;
using Xunit;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.Domain.Tests.ValueObjects
{
    public class ExpenseTitleTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void should_fail_validation_for_empty_or_whitespace_title(string title)
        {
            var exception = Assert.Throws<DomainException>(() => new ExpenseTitle(title));
            exception.ErrorCode.Should().Be(ErrorCodes.InvalidExpenseTitle);
        }

        [Fact]
        public void should_pass_validation_for_correct_title()
        {
            new ExpenseTitle("correct title");
        }
    }
}
