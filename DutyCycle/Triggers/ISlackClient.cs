using System.Threading.Tasks;

namespace DutyCycle.Triggers
{
    public interface ISlackClient
    {
        Task SendMessageToChannel(string channelId, string message);

        Task AddUserToGroup(string memberId, string groupId);

        Task RemoveUserToGroup(string memberId, string groupId);
    }
}