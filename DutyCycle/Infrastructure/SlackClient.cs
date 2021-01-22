using System.Threading.Tasks;
using DutyCycle.Triggers;

namespace DutyCycle.Infrastructure
{
    public class SlackClient : ISlackClient
    {
        public Task SendMessageToChannel(string channelId, string message)
        {
            // todo: implement
            return Task.CompletedTask;
        }

        public Task AddUserToGroup(string memberId, string groupId)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveUserToGroup(string memberId, string groupId)
        {
            throw new System.NotImplementedException();
        }
    }
}