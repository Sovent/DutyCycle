using System;

namespace DutyCycle.Groups.Domain.Slack
{
    public interface IAddToSlackLinkProvider
    {
        string GetLink(Guid slackConnectionIdentifier);
    }
}