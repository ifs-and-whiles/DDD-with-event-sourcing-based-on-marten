using System;

namespace DDDWithEventSourcingBasedOnMarten.EventSourcing
{
    public class EventStoreException : Exception
    {
        public EventStoreException(string message) : base(message)
        {
            
        }
    }
}