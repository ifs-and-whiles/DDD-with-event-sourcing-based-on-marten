using System;
using System.Collections.Generic;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public partial class Events
    {
        public partial class Expenses
        {
            public static class V1
            {
                [Event]
                public class ExpenseTagsAssigned
                {
                    public ExpenseTagsAssigned(
                        Guid expenseId,
                        List<string> tags)
                    {
                        Tags = tags;
                        ExpenseId = expenseId;
                    }

                    public Guid ExpenseId { get; }
                    
                    public List<string> Tags { get; }

                    public override string ToString()
                        => $"{nameof(ExpenseTagsAssigned)}";
                }
                
                [Event]
                public class ExpenseAdded
                {
                    public Guid ExpenseId { get; }
                    public DateTimeOffset Date { get; }
                    public DateTimeOffset CreationDate { get; }
                    public string Seller { get; }
                    public string Title { get; }
                    public decimal TotalAmount { get; }
                    public decimal? Quantity { get; }
                    public decimal? UnitPrice { get; }
                    public List<string> Tags { get; }
                    public string Comments { get; }

                    public override string ToString()
                        => $"{nameof(ExpenseAdded)}";

                    public ExpenseAdded(
                        Guid expenseId,
                        DateTimeOffset date, 
                        string seller, 
                        string title, 
                        decimal totalAmount, 
                        decimal? quantity, 
                        decimal? unitPrice, 
                        List<string> tags, 
                        string comments, 
                        DateTimeOffset creationDate)
                    {
                        ExpenseId = expenseId;
                        Date = date;
                        Seller = seller;
                        Title = title;
                        TotalAmount = totalAmount;
                        Quantity = quantity;
                        UnitPrice = unitPrice;
                        Tags = tags ?? new List<string>();
                        Comments = comments;
                        CreationDate = creationDate;
                    }
                    
                    
                }
                
                
            }
        }
    }
}