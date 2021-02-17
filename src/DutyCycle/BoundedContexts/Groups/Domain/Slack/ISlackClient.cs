using System.Threading.Tasks;

namespace DutyCycle.Groups.Domain.Slack
{
    public interface ISlackClient
    {
        Task SendMessageToChannel(string channelId, string message);

        Task AddUserToGroup(string memberId, string groupId);

        Task RemoveUserToGroup(string memberId, string groupId);
    }
}