using System;
using System.Threading.Tasks;
using DDDWithEventSourcingBasedOnMarten.Domain;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;
using static DDDWithEventSourcingBasedOnMarten.Expenses.Contracts.Commands.Expenses;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.API.Commands
{
    public class CreateExpenseCommandHandler
    {
        private readonly ApplicationService<Expense, ExpenseStreamId> _applicationService;

        public CreateExpenseCommandHandler(
            ApplicationService<Expense,ExpenseStreamId> applicationService)
        {
            _applicationService = applicationService;
        }
        
        public async Task<V1.CreateResponse> Handle(V1.Create command)
        {
            var expenseId = Guid.NewGuid();

            await _applicationService.Create(
                cmd => ExpenseStreamId.From(ExpenseId.From(expenseId)),
                (cmd) => Expense.Create(
                    ExpenseId.From(expenseId),
                    ExpenseDate.From(cmd.Date),
                    ExpenseTitle.From(cmd.Title),
                    ExpenseTotalAmount.From(cmd.TotalAmount),
                    ExpenseSeller.From( cmd.Seller),
                    Tags.From(cmd.Tags),
                    CreationDate.From(DateTimeOffset.UtcNow),
                    ExpenseQuantity.From(cmd.Quantity),
                    ExpenseUnitPrice.From(cmd.UnitPrice),
                    Comments.From(cmd.Comments)),
                command);

            return new V1.CreateResponse()
            {
                ExpenseId = expenseId
            };
        }
    }
}