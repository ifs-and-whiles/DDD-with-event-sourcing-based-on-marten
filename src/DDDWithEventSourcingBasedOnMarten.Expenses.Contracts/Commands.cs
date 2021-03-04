using System;
using System.Collections.Generic;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.Contracts
{
    public partial class Commands
    {
        public partial class Expenses
        {
            public static class V1
            {
                public class Create
                {
                    public DateTimeOffset Date { get; set; }
                    public Guid? ReceiptId { get; set; }
                    public string Seller { get; set; }
                    public string Title { get; set; }
                    public decimal TotalAmount { get; set; }
                    public decimal? Quantity { get; set; }
                    public decimal? UnitPrice { get; set; }
                    public List<string> Tags { get; set; }
                    public string Comments { get; set; }
                }
                
                public class CreateResponse
                {
                    public Guid ExpenseId { get; set; }
                }
                
                public class AssignTags
                {
                    public Guid Id { get; set; }
                    public List<string> Tags { get; set; }
                }
            }
        }
    }
}