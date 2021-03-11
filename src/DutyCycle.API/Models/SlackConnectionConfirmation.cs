using System;

namespace DutyCycle.API.Models
{
    public class SlackConnectionConfirmation
    {
        public Guid ConnectionId { get; set; }
        
        public string AuthenticationCode { get; set; }
    }
}