using System;

namespace DutyCycle.Common
{
    public abstract class DomainError<T> where T : DomainError<T>
    {
        protected DomainError(DateTimeOffset occuredOnUtc)
        {
            OccuredOnUtc = occuredOnUtc;
        }
        
        public abstract string Description { get; }
        public DateTimeOffset OccuredOnUtc { get; }

        public DomainException<T> ToException() => new DomainException<T>((T)this);
    }
}