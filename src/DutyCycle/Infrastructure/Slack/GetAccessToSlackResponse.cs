using System.Text.Json.Serialization;

namespace DutyCycle.Infrastructure.Slack
{
    public class GetAccessToSlackResponse
    {
        [JsonPropertyName("ok")]
        public bool IsSuccess { get; set; }
        
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}