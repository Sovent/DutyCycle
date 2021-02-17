using System;
using DutyCycle.Common;

namespace DutyCycle.Groups.Domain.Slack
{
    public class SlackConnectionNotFound : DomainError<SlackConnectionNotFound>
    {
        public SlackConnectionNotFound(Guid connectionId, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            Description = $"Slack connection match identifier {connectionId} not found";
        }

        public SlackConnectionNotFound(int organizationId, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            Description = $"Slack connection for organization {organizationId} not found";
        }
        
        public override string Description { get; }
    }
}