using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static DDDWithEventSourcingBasedOnMarten.Expenses.Contracts.Commands.Expenses;
namespace DDDWithEventSourcingBasedOnMarten.Expenses.API.Commands
{
    [ApiController, Route(ExpensesApiRouteConsts.Prefix + "/v1/expenses/commands")]
    public class ExpenseCommandApiController
    {
        private readonly CreateExpenseCommandHandler _createExpenseCommandHandler;
        private readonly AssignTagsCommandHandler _assignTagsCommandHandler;

        public ExpenseCommandApiController(
            CreateExpenseCommandHandler createExpenseCommandHandler, 
            AssignTagsCommandHandler assignTagsCommandHandler)
        {
            _createExpenseCommandHandler = createExpenseCommandHandler;
            _assignTagsCommandHandler = assignTagsCommandHandler;
        }
        
        [HttpPost, Route("create")]
        public async Task<V1.CreateResponse> CreateExpense(V1.Create command)
        {
            return await _createExpenseCommandHandler.Handle(command);
        }

        [HttpPost, Route("assign-tags")]
        public async Task AssignTags(V1.AssignTags command)
        {
            await _assignTagsCommandHandler.Handle(command);
        }
    }
}