using System;
using System.Collections.Generic;
using AutoFixture;
using DDDWithEventSourcingBasedOnMarten.Domain;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;
using FluentAssertions;
using Xunit;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.Domain.Tests.Aggregates
{
    public class ExpenseAggregateTests
    {
        public Fixture Fixture { get; } = new Fixture();

        [Fact]
        public void can_add_new_expense()
        {
            //given
            var id= Fixture.Create<ExpenseId>();
            var date= Fixture.Create<ExpenseDate>();
            var title= Fixture.Create<ExpenseTitle>();
            var totalAmount= Fixture.Create<ExpenseTotalAmount>();
            var seller= Fixture.Create<ExpenseSeller>();
            var tags= Fixture.Create<Tags>();
            var quantity= Fixture.Create<ExpenseQuantity>();
            var unitPrice= Fixture.Create<ExpenseUnitPrice>();
            var comments = Fixture.Create<Comments>();
            var creationDate = Fixture.Create<CreationDate>();

            //when
            var expense = Expense.Create(
                id, date, title,
                totalAmount, seller, tags, creationDate, quantity, unitPrice,
                comments);

            //then
            expense.GetChanges().Should().BeEquivalentTo(new Events.Expenses.V1.ExpenseAdded(
                expenseId: id.Value,
                date: date.Value,
                title: title.Value,
                seller: seller.Name.ValueOrNull(),
                totalAmount: totalAmount.Value,
                quantity: quantity.Value,
                unitPrice: unitPrice.Value,
                tags: tags.ToStringList(),
                comments: comments.Value,
                creationDate: creationDate
            ));
        }
        

        [Fact]
        public void can_assign_tags()
        {
            //given
            var expense = new Expense();
            var expenseAdded = Fixture
                .Build<Events.Expenses.V1.ExpenseAdded>()
                .With(x=> x.Tags, new List<string>() { "tag-1" })
                .Create();
            expense.Load(new[] {new StoredEvent(1, "test-stream", expenseAdded, DateTime.UtcNow) });
            
            //when
            var newTags = Fixture.Create<Tags>();
            
            expense.AssignTags(newTags);
            
            //then
            expense.GetChanges().Should().BeEquivalentTo(new Events.Expenses.V1.ExpenseTagsAssigned(
                expenseId: expenseAdded.ExpenseId,
                tags: newTags.ToStringList()));
        }

    }
}
