using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;

namespace DutyCycle.Groups.Domain.Slack
{
    public class NotConnectedSlackClient : ISlackClient
    {
        public NotConnectedSlackClient(int organizationId)
        {
            _organizationId = organizationId;
        }

        public async Task<OrganizationSlackInfo> GetInfo()
        {
            ThrowConnectionNotFound();
            return null;
        }

        public async Task SendMessageToChannel(string channelId, string message)
        {
            ThrowConnectionNotFound();
        }

        public async Task AddUserToGroup(string memberId, string groupId)
        {
            ThrowConnectionNotFound();
        }

        public async Task RemoveUserToGroup(string memberId, string groupId)
        {
            ThrowConnectionNotFound();
        }

        private void ThrowConnectionNotFound()
        {
            throw new SlackConnectionNotFound(_organizationId, DateTimeOffset.UtcNow).ToException();
        }
        
        private readonly int _organizationId;
    }
}