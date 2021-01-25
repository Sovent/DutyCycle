using System;

namespace DutyCycle.Triggers
{
    public class TriggersContext
    {
        public TriggersContext(ISlackClient slackClient, ISlackMessageTemplater slackMessageTemplater)
        {
            SlackClient = slackClient ?? throw new ArgumentNullException(nameof(slackClient));
            SlackMessageTemplater =
                slackMessageTemplater ?? throw new ArgumentNullException(nameof(slackMessageTemplater));
        }

        public ISlackClient SlackClient { get; }
        
        public ISlackMessageTemplater SlackMessageTemplater { get; }
    }
}