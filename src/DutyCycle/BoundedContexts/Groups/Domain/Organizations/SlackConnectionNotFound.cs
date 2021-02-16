using System;
using DutyCycle.Common;

namespace DutyCycle.Groups.Domain.Organizations
{
    public class SlackConnectionNotFound : DomainError<SlackConnectionNotFound>
    {
        public SlackConnectionNotFound(Guid connectionId, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            Description = $"Slack connection match identifier {connectionId} not found";
        }

        public override string Description { get; }
    }
}