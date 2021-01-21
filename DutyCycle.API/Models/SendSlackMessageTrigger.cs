namespace DutyCycle.API.Models
{
    public class SendSlackMessageTrigger : GroupActionTrigger
    {
        public string ChannelId { get; set; }
        
        public string MessageTextTemplate { get; set; }
    }
}