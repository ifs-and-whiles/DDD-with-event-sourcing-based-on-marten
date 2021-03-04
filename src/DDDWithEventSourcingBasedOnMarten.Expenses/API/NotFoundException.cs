using System;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.API
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
            
        }

        public NotFoundException(string message): base(message)
        {
            
        }
    }
}