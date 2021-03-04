namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class ExpenseStreamId : Value<ExpenseStreamId>
    {
        public string Value { get; }

        public static ExpenseStreamId From(ExpenseId expenseId) =>
            new ExpenseStreamId($"Expense-{expenseId}");

        public ExpenseStreamId(string value)
        {
            if (string.IsNullOrEmpty(value) || !value.StartsWith("Expense-"))
                throw new DomainException($"Invalid value {value} for expense stream id, it has to start with 'Expense-' prefix", ErrorCodes.ExpenseStreamIdInvalid);

            Value = value;
        }

        public static implicit operator string(ExpenseStreamId self) => self.Value;
    }
}