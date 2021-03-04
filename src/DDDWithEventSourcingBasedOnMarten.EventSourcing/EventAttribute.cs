using System;

namespace DDDWithEventSourcingBasedOnMarten.EventSourcing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventAttribute : Attribute { }
}