using DutyCycle.Groups.Domain.Slack;
using LanguageExt;
using SlackAPI;

namespace DutyCycle.Infrastructure.Slack
{
    public class SlackClientFactory : ISlackClientFactory
    {
        public ISlackClient Create(int organizationId, Option<SlackConnection> slackConnection)
        {
            return slackConnection
                .Bind(connection => connection.AccessToken)
                .Match(
                    accessToken => new SlackClient(new SlackTaskClient(accessToken)),
                    () => (ISlackClient)new NotConnectedSlackClient(organizationId));
        }
    }
}