using System;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class ExpenseDate : Value<ExpenseDate>
    {
        public DateTimeOffset Value { get; }

        public ExpenseDate(DateTimeOffset value)
        {
            Validate(value);
            Value = value;
        }

        private void Validate(DateTimeOffset value)
        {
            if(!(value.DateTime > DateTime.MinValue && value.DateTime < DateTime.MaxValue))
                throw new DomainException(
                    $"Expense date {value} should be greater than {DateTime.MinValue} and less than {DateTime.MaxValue}",
                    ErrorCodes.InvalidExpenseDate);
        }

        public static implicit operator DateTimeOffset(ExpenseDate date) => date.Value;

        public static ExpenseDate From(DateTimeOffset value)
        {
            return new ExpenseDate(value);
        }
    }
}
