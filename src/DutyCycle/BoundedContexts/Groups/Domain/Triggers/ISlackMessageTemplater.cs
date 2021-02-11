using System;

namespace DutyCycle.Groups.Domain.Triggers
{
    public interface ISlackMessageTemplater
    {
        string CreateFromTemplate(string template, GroupInfo groupInfo, DateTimeOffset messageCreationDateTime);
    }
}