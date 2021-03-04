using System;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; }
        
        public DomainException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}