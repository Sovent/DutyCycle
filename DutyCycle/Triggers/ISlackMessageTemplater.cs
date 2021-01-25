using System;

namespace DutyCycle.Triggers
{
    public interface ISlackMessageTemplater
    {
        string CreateFromTemplate(string template, GroupInfo groupInfo, DateTimeOffset messageCreationDateTime);
    }
}