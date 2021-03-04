using Autofac;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;
using DDDWithEventSourcingBasedOnMarten.Infrastructure.Config;
using DDDWithEventSourcingBasedOnMarten.Infrastructure.Database;
using DDDWithEventSourcingBasedOnMarten.Marten;
using Marten;

namespace DDDWithEventSourcingBasedOnMarten
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            // Event Store
            builder.RegisterType<MartenAggregateStore>().As<IAggregateStore>();
            
            // Read DB
            builder.Register(ctx => DocumentStoreFactory.Create(ctx.Resolve<DatabaseConfig>()))
                .As<IDocumentStore>()
                .SingleInstance();


        }
    }
}