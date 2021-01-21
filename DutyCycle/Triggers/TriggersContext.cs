using System;

namespace DutyCycle.Triggers
{
    public class TriggersContext
    {
        public TriggersContext(ISlackClient slackClient)
        {
            SlackClient = slackClient ?? throw new ArgumentNullException(nameof(slackClient));
        }

        public ISlackClient SlackClient { get; }
    }
}