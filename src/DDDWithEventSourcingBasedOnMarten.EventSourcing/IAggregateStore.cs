using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDWithEventSourcingBasedOnMarten.EventSourcing
{
    public interface IAggregateStore
    {
        Task<bool> Exists<TIdType>(TIdType streamId);

        Task Save<TAggregateRoot, TIdType>(TAggregateRoot aggregate) where TAggregateRoot : AggregateRoot<TIdType>;

        Task<TAggregateRoot> Load<TAggregateRoot, TIdType>(TIdType streamId) where TAggregateRoot : AggregateRoot<TIdType>;

    }
}