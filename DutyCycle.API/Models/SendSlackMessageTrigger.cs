namespace DutyCycle.API.Models
{
    public class SendSlackMessageTrigger : RotationChangedTrigger
    {
        public string ChannelId { get; set; }
        
        public string MessageTextTemplate { get; set; }
    }
}