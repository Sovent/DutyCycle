using System;
using System.Runtime.Serialization;

namespace DutyCycle.Common
{
    [Serializable]
    public class DomainException<TError> : Exception where TError : DomainError<TError>
    {
        public DomainException()
        {
        }

        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception inner) : base(message, inner)
        {
        }

        public DomainException(TError error) : base(error?.Description)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }
        
        public TError Error { get; }

        protected DomainException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}