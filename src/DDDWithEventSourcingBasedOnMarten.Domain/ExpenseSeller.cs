using System;
using Billy.CodeReadability;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class ExpenseSeller : Value<ExpenseSeller>
    {

        public Option<SellerName> Name { get; }

        public ExpenseSeller(
            Option<SellerName> name)
        {
            Name = name;
        }


        public static ExpenseSeller From(
            string name) =>
            new ExpenseSeller(SellerName.From(name));

    }
    

    public class SellerName : Value<SellerName>
    {
        public string Value { get; }

        public SellerName(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public static Option<SellerName> From(string value) => string.IsNullOrWhiteSpace(value)
            ? Option<SellerName>.None
            : new SellerName(value);
    }


    public static class ExpenseSellerExtensions
    {
        public static string ValueOrNull(this Option<SellerName> name) =>
            name.Match(value => value.Value, () => null);
        
    }
}
