using System.Collections.Generic;

namespace DDDWithEventSourcingBasedOnMarten.EventSourcing
{
    public abstract class AggregateRoot<TIdType> 
    {
        private readonly List<object> _publishedEvents = new List<object>();

        public TIdType Id { get; protected set; }

        public long Version { get; private set; } = 0;

        void Handle(object @event) => When(@event);

        protected abstract void When(object @event);

        protected void Apply(object @event)
        {
            When(@event);
            Version++;
            EnsureValidState();
            _publishedEvents.Add(@event);
        }

        public object[] GetChanges() => _publishedEvents.ToArray();

        public void Load(IEnumerable<StoredEvent> history)
        {
            foreach (var e in history)
            {
                When(e.Data);
                Version = e.Version;
            }
        }

        public void ClearChanges() => _publishedEvents.Clear();

        protected abstract void EnsureValidState();
    }
}