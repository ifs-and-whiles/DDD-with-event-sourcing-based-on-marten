using Billy.CodeReadability;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class Expense : AggregateRoot<ExpenseStreamId>
    {
        public ExpenseId ExpenseId { get; private set; }
        public Tags Tags { get; private set; }
        
        public static Expense Create(
            ExpenseId id,
            ExpenseDate date,
            ExpenseTitle title,
            ExpenseTotalAmount totalAmount,
            ExpenseSeller seller,
            Tags tags,
            CreationDate creationDate,

            Option<ExpenseQuantity> quantity,
            Option<ExpenseUnitPrice> unitPrice,
            Option<Comments> comments)
        {

            var expense = new Expense();

            expense.Apply(new Events.Expenses.V1.ExpenseAdded(
                expenseId: id,
                date: date,
                title: title,
                totalAmount: totalAmount,
                seller: seller.Name.ValueOrNull(),
                quantity: quantity.ValueOrNull(),
                unitPrice: unitPrice.ValueOrNull(),
                tags: tags.ToStringList(),
                comments: comments.ValueOrNull(),
                creationDate: creationDate));

            return expense;
        }


        public void AssignTags(Tags tagsToAssign)
        {
            Tags.CanAssign(tagsToAssign)
                .Match(
                    tagsCanBeAssigned => 
                        Apply(new Events.Expenses.V1.ExpenseTagsAssigned(ExpenseId, tagsToAssign.ToStringList())),

                    atLeastOneTagAlreadyAssigned => 
                        throw new DomainException(
                            $"Tags cannot be assigned to expense {ExpenseId}. At least one tag from tags to assign is already assigned, cannot assigned the same tags twice", 
                            ErrorCodes.AtLeastOneTagFromTagsToAssignIsAlreadyAssigned));
            
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.Expenses.V1.ExpenseAdded e:
                    Id = ExpenseStreamId.From(ExpenseId.From(e.ExpenseId));
                    ExpenseId = ExpenseId.From(e.ExpenseId);
                    Tags = Tags.From(e.Tags);
                    break;
                case Events.Expenses.V1.ExpenseTagsAssigned e:
                    Tags += Tags.From(e.Tags);
                    break;
                
            }
        }

        protected override void EnsureValidState()
        {

        }
    }
}
