using System;
using DutyCycle.Common;

namespace DutyCycle.Groups.Domain
{
    public class SlackInteractionFailed : DomainError<SlackInteractionFailed>
    {
        public SlackInteractionFailed(string slackProvidedError, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            Description = "Interaction with slack failed with error: " + slackProvidedError;
        }

        public override string Description { get; }
    }
}