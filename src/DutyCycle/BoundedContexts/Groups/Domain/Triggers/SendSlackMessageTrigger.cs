using System;
using System.Threading.Tasks;

namespace DutyCycle.Groups.Domain.Triggers
{
    public class SendSlackMessageTrigger : RotationChangedTrigger
    {
        public SendSlackMessageTrigger(Guid id, string channelId, string messageTextTemplate) 
            : base(id)
        {
            ChannelId = channelId;
            MessageTextTemplate = messageTextTemplate;
        }

        public string ChannelId { get; private set; }
        
        public string MessageTextTemplate { get; private set; }
        
        public override async Task Run(GroupInfo group, TriggersContext triggersContext)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            if (triggersContext == null) throw new ArgumentNullException(nameof(triggersContext));
            
            var slackClient = triggersContext.SlackClient;
            var messageText =
                triggersContext.SlackMessageTemplater.CreateFromTemplate(
                    MessageTextTemplate,
                    group,
                    DateTimeOffset.UtcNow);
            await slackClient.SendMessageToChannel(ChannelId, messageText);
        }
    }
}