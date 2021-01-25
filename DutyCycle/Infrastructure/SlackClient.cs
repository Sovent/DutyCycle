using System;
using System.Threading.Tasks;
using DutyCycle.Triggers;
using SlackAPI;

namespace DutyCycle.Infrastructure
{
    public class SlackClient : ISlackClient
    {
        private readonly SlackTaskClient _slackTaskClient;

        public SlackClient(SlackTaskClient slackTaskClient)
        {
            _slackTaskClient = slackTaskClient ?? throw new ArgumentNullException(nameof(slackTaskClient));
        }
        
        public async Task SendMessageToChannel(string channelId, string message)
        {
            if (channelId == null) throw new ArgumentNullException(nameof(channelId));
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            await _slackTaskClient.PostMessageAsync(channelId, message);
        }

        public async Task AddUserToGroup(string memberId, string groupId)
        {
        }

        public Task RemoveUserToGroup(string memberId, string groupId)
        {
            throw new System.NotImplementedException();
        }
    }
}