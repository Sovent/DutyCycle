using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;
using DutyCycle.Groups.Domain.Slack;
using SlackAPI;

namespace DutyCycle.Infrastructure.Slack
{
    public class SlackClient : ISlackClient
    {
        private readonly SlackTaskClient _slackTaskClient;

        public SlackClient(SlackTaskClient slackTaskClient)
        {
            _slackTaskClient = slackTaskClient ?? throw new ArgumentNullException(nameof(slackTaskClient));
        }

        public Task<OrganizationSlackInfo> GetInfo()
        {
            return Task.FromResult(new OrganizationSlackInfo(_slackTaskClient.MyTeam.name));
        }

        public async Task SendMessageToChannel(string channelId, string message)
        {
            if (channelId == null) throw new ArgumentNullException(nameof(channelId));
            if (message == null) throw new ArgumentNullException(nameof(message));

            PostMessageResponse response;
            try
            {
                response = await _slackTaskClient.PostMessageAsync(channelId, message);
            }
            catch (Exception exception)
            {
                throw new SlackInteractionFailed(exception.Message, DateTimeOffset.Now).ToException();
            }
            
            if (!response.ok)
            {
                throw new SlackInteractionFailed(response.error, DateTimeOffset.Now).ToException();
            }
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