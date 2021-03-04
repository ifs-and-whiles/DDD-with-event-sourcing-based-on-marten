using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Linq.Expressions;
using Baseline.Reflection;
using DDDWithEventSourcingBasedOnMarten.Domain;
using DDDWithEventSourcingBasedOnMarten.Infrastructure.Config;
using Marten;
using Marten.Events;
using Marten.Schema.Identity;

namespace DDDWithEventSourcingBasedOnMarten.Infrastructure.Database
{
    public class DocumentStoreFactory
    {
        public static IDocumentStore Create(DatabaseConfig config)
        {
            return DocumentStore.For(_ =>
            {
                _.Connection(config.ConnectionString);
                _.UseDefaultSerialization(casing: Casing.CamelCase);
                _.DefaultIdStrategy = (mapping, storeOptions) => new CombGuidIdGeneration();
                
               //_.Storage.MappingFor(typeof(Expenses.Contracts.Queries.Expenses.V1.ReadModels.Expense));

                // var columns = new Expression<Func<Expenses.Contracts.Queries.Expenses.V1.ReadModels.Expense, object>>[]
                // {
                //     x => x.OwnerId,
                //     x => x.Id
                // };
                //_.Schema.For<Expenses.Contracts.Queries.Expenses.V1.ReadModels.Expense>().Index(columns);

                _.Events.StreamIdentity = StreamIdentity.AsString;
                
                //_.Events.AsyncProjections.Add(new ExpenseProjection());

                _.Events.AddEventTypes(GetEventTypes().ToList());
            });
        }
        
        private static IEnumerable<Type> GetEventTypes()
        {
            var assemblies = new[]
            {
                typeof(Events.Expenses.V1.ExpenseAdded).Assembly
            };
            
            foreach (var assembly in assemblies)
            {
                foreach (var @event in assembly.GetTypes().Where(t => t.HasAttribute<EventAttribute>()))
                {
                    yield return @event;
                }
            }
        } 
    }
}