using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDWithEventSourcingBasedOnMarten.EventSourcing;
using Marten;

namespace DDDWithEventSourcingBasedOnMarten.Marten
{
    public class MartenAggregateStore: IAggregateStore
    {
        private readonly IDocumentStore _store;

        public MartenAggregateStore(IDocumentStore store)
        {
            _store = store;
        }

        public async Task<bool> Exists<TIdType>(TIdType streamId)
        {
            using var session = _store.LightweightSession();
            
            var state = await session.Events.FetchStreamStateAsync(streamId.ToString());
            
            return state != null;
        }

        public async Task Save<TAggregateRoot, TIdType>(TAggregateRoot aggregate) where TAggregateRoot : AggregateRoot<TIdType>
        {
            using var session = _store.LightweightSession();

            var streamName = aggregate.Id.ToString();
            
            session.Events.Append(streamName, (int)aggregate.Version, aggregate.GetChanges());

            await session.SaveChangesAsync();
        }

        public async Task<TAggregateRoot> Load<TAggregateRoot, TIdType>(TIdType streamId) where TAggregateRoot : AggregateRoot<TIdType>
        {    
            using var session = _store.LightweightSession();
            
            var aggregate = (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), true);
            
            var events = await session.Events.FetchStreamAsync(streamId.ToString());
            
            aggregate.Load(events.Select(@event => new StoredEvent(
                streamId: @event.StreamKey,
                version: @event.Version,
                data: @event.Data,
                timestampUtc: @event.Timestamp.UtcDateTime
            )));
                
            return aggregate;
        }


    }
}