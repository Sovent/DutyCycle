using System;

namespace DutyCycle.Groups.Domain.Organizations
{
    public interface IAddToSlackLinkProvider
    {
        string GetLink(Guid slackConnectionIdentifier);
    }
}