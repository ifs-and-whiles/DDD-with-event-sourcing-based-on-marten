using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDWithEventSourcingBasedOnMarten.EventSourcing
{
    public class ApplicationService<TAggregate, TIdType> where TAggregate : AggregateRoot<TIdType>
    {
        readonly Dictionary<Type, Func<object, Task>> _handlers =
            new Dictionary<Type, Func<object, Task>>();

        readonly IAggregateStore _store;

        public ApplicationService(IAggregateStore store) => _store = store;
        
        public async Task<bool> Exists(TIdType aggregateId) =>
            await _store.Exists(aggregateId);
        
        public async Task Create<TCommand>(
            Func<TCommand, TIdType> getAggregateId,
            Func<TCommand, TAggregate> creator,
            TCommand command)
            where TCommand : class
        {
            var aggregateId = getAggregateId(command);

            if (await _store.Exists(aggregateId))
                throw new EventStoreException(
                    $"Entity with id {aggregateId.ToString()} already exists");

            var aggregate = creator(command);
            
            await _store.Save<TAggregate, TIdType>(aggregate);

        }

        public async Task Update<TCommand>(
            Func<TCommand, TIdType> getAggregateId,
            Action<TAggregate, TCommand> updater,
            TCommand command) where TCommand : class
        {
            var aggregateId = getAggregateId(command);

            if (!await _store.Exists(aggregateId))
                throw new EventStoreException(
                    $"Entity with id {aggregateId.ToString()} cannot be found");

            var aggregate = await _store.Load<TAggregate, TIdType>(aggregateId);

            updater(aggregate, command);
            await _store.Save<TAggregate, TIdType>(aggregate);
        }


        public async Task Update<TCommand>(
            Func<TCommand, TIdType> getAggregateId,
            Func<TAggregate, TCommand, Task> updater,
            TCommand command) where TCommand : class
        {
            var aggregateId = getAggregateId(command);

            if (!await _store.Exists(aggregateId))
                throw new EventStoreException(
                    $"Entity with id {aggregateId.ToString()} cannot be found");

            var aggregate = await _store.Load<TAggregate, TIdType>(aggregateId);

            await updater(aggregate, command);
            await _store.Save<TAggregate, TIdType>(aggregate);
        }
    }
}