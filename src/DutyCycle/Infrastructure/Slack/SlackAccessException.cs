using System;
using System.Runtime.Serialization;

namespace DutyCycle.Infrastructure.Slack
{
    [Serializable]
    public class SlackAccessException : Exception
    {
        public SlackAccessException()
        {
        }

        public SlackAccessException(string message) : base(message)
        {
        }

        public SlackAccessException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SlackAccessException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}