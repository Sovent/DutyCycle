using System;

namespace DutyCycle.Triggers
{
    public class TriggersTooling
    {
        public TriggersTooling(ISlackClient slackClient)
        {
            SlackClient = slackClient ?? throw new ArgumentNullException(nameof(slackClient));
        }

        public ISlackClient SlackClient { get; }
    }
}