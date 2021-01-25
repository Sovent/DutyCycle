using System;
using System.Threading.Tasks;

namespace DutyCycle.Triggers
{
    public class SendSlackMessageTrigger : GroupActionTrigger
    {
        public SendSlackMessageTrigger(Guid id, GroupAction action, string channelId, string messageTextTemplate) 
            : base(id, action)
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
            var messageText = triggersContext.SlackMessageTemplater.CreateFromTemplate(MessageTextTemplate, group);
            await slackClient.SendMessageToChannel(ChannelId, messageText);
        }
    }
}