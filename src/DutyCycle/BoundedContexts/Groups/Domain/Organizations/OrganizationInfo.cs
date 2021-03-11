using System;
using LanguageExt;

namespace DutyCycle.Groups.Domain.Organizations
{
    public class OrganizationInfo
    {
        public OrganizationInfo(int id, string name, Option<OrganizationSlackInfo> slackInfo)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SlackInfo = slackInfo;
        }

        public int Id { get; }
        
        public string Name { get; }
        
        public Option<OrganizationSlackInfo> SlackInfo { get; }
    }
}