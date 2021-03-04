using System;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class ExpenseId : Value<ExpenseId>
    {
        public Guid Value { get;  }
        public ExpenseId(Guid value)
        {
            Value = value;
        } 

        public static ExpenseId From(Guid value)
            => new ExpenseId(value);
        
        public static implicit operator Guid(ExpenseId self) => self.Value;

        public override string ToString() => Value.ToString();
    }
}
