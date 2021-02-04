using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DutyCycle.Triggers;

namespace DutyCycle.Infrastructure.Slack
{
    public class SlackMessageTemplater : ISlackMessageTemplater
    {
        public const string CurrentDutiesPlaceholder = "${CurrentDuties}";
        public const string NextDutiesPlaceholder = "${NextDuties}";
        public const string AllMembersPlaceholder = "${AllMembers}";
        public const string NextRotationPlaceholder = "${NextRotationDateTimeUtc}";
        public const string SlackLineBreak = "\n";
        
        public string CreateFromTemplate(string template, GroupInfo groupInfo, DateTimeOffset messageCreationDateTime)
        {
            var result = new StringBuilder(template);

            void FillPlaceholderIfPresent(string placeholder, Func<string> getValue)
            {
                if (template.Contains(placeholder))
                {
                    result.Replace(placeholder, getValue());
                }
            }

            FillPlaceholderIfPresent(
                NextRotationPlaceholder,
                () => groupInfo.CyclingCronExpression
                    .GetNextOccurrence(messageCreationDateTime.UtcDateTime)
                    .ToString());
            
            void FillGroupMembersPlaceholders(string placeholder, IEnumerable<GroupMemberInfo> groupMembers) => 
                FillPlaceholderIfPresent(placeholder, () => JoinMembers(groupMembers));

            FillGroupMembersPlaceholders(CurrentDutiesPlaceholder, groupInfo.CurrentDuties);
            FillGroupMembersPlaceholders(NextDutiesPlaceholder, groupInfo.NextDuties);
            FillGroupMembersPlaceholders(AllMembersPlaceholder, groupInfo.AllMembers);

            return result.ToString();
        }

        private static string JoinMembers(IEnumerable<GroupMemberInfo> members)
        {
            return string.Join(
                SlackLineBreak, 
                members.Select((member, index) => $"    {index + 1}.{member.Name}"));
        }
    }
}