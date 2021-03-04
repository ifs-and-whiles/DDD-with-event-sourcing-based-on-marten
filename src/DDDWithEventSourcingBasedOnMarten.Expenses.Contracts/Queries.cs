using System;
using System.Collections.Generic;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.Contracts
{
    public partial class Queries
    {
        public partial class Expenses
        {
            public static class V1
            {
                public class GetExpense
                {
                    public Guid Id { get; set; }
                }

                public static class ReadModels
                {
                    public class Expense
                    {
                        public Guid Id { get; set; }
                        public DateTimeOffset Date { get; set; }
                        public string Title { get; set; }
                        public decimal TotalAmount { get; set; }
                        public decimal? Quantity { get; set; }
                        public decimal? UnitPrice { get; set; }
                        public List<string> Tags { get; set; }
                        public string Seller { get; set; }
                        public string Comments { get; set; }
                    } 
                }
            }
        }
    }
}