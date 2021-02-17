using System;

namespace DutyCycle.Groups.Domain.Slack
{
    public interface ISlackMessageTemplater
    {
        string CreateFromTemplate(string template, GroupInfo groupInfo, DateTimeOffset messageCreationDateTime);
    }
}