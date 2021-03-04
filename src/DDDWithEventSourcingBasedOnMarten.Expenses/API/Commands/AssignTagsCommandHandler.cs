using System.Threading.Tasks;
using DDDWithEventSourcingBasedOnMarten.Domain;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;
using static DDDWithEventSourcingBasedOnMarten.Expenses.Contracts.Commands.Expenses;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.API.Commands
{
    public class AssignTagsCommandHandler
    {
        private readonly ApplicationService<Expense, ExpenseStreamId> _applicationService;

        public AssignTagsCommandHandler(ApplicationService<Expense, ExpenseStreamId> applicationService)
        {
            _applicationService = applicationService;
        }

        public async Task Handle(V1.AssignTags command)
        {
            await _applicationService.Update(
                cmd => ExpenseStreamId.From(ExpenseId.From(command.Id)),
                (expense, cmd) => expense.AssignTags(
                    Tags.From(cmd.Tags)),
                command);
        }
    }
}