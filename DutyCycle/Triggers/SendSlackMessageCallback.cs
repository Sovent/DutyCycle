using System;
using System.Threading.Tasks;

namespace DutyCycle.Triggers
{
    public class SendSlackMessageCallback : TriggerCallback
    {
        public SendSlackMessageCallback(Guid id, string channelId, string messageTextTemplate) : base(id)
        {
            ChannelId = channelId;
            MessageTextTemplate = messageTextTemplate;
        }

        public string ChannelId { get; private set; }
        
        public string MessageTextTemplate { get; private set; }
        
        public override async Task Execute(Group group, TriggersTooling triggersTooling)
        {
            var slackClient = triggersTooling.SlackClient;
            var messageText = FillTemplate(group);
            await slackClient.SendMessageToChannel(ChannelId, messageText);
        }

        private string FillTemplate(Group group)
        {
            // todo: implement templater
            return MessageTextTemplate;
        }
    }
}