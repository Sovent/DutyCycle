using DutyCycle.Groups.Domain.Organizations;
using LanguageExt;

namespace DutyCycle.Groups.Domain.Slack
{
    public interface ISlackClientFactory
    {
        ISlackClient Create(int organizationId, Option<SlackConnection> slackConnection);
    }
}