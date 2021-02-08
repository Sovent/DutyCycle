using System;
using System.Runtime.Serialization;

namespace DutyCycle.API.Authentication
{
    [Serializable]
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException()
        {
        }

        public AuthenticationFailedException(string message) : base(message)
        {
        }

        public AuthenticationFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AuthenticationFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}