using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;

namespace DutyCycle.Groups.Domain.Slack
{
    public interface ISlackClient
    {
        Task<OrganizationSlackInfo> GetInfo();
        
        Task SendMessageToChannel(string channelId, string message);

        Task AddUserToGroup(string memberId, string groupId);

        Task RemoveUserToGroup(string memberId, string groupId);
    }
}