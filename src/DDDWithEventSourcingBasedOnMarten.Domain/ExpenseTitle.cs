using Billy.CodeReadability;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class ExpenseTitle : Value<ExpenseTitle>
    {
        public string Value { get; }

        public ExpenseTitle(string value)
        {
            Validate(value);
            Value = value;
        }

        private void Validate(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new DomainException(
                    "Expense title can not be empty or white space",
                    ErrorCodes.InvalidExpenseTitle);
        }

        public static implicit operator string(ExpenseTitle title) => title.Value;
        public static ExpenseTitle From(string title) => new ExpenseTitle(title);
    }

    public static class ExpenseTitleExtensions
    {
        public static string ValueOrNull(this Option<ExpenseTitle> expenseTitle) =>
            expenseTitle.Match(value => value.Value, () => null);
    }
}
