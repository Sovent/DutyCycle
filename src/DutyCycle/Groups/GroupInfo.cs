using System.Collections.Generic;
using System.Linq;
using Cronos;
using DutyCycle.Triggers;

namespace DutyCycle
{
    public class GroupInfo
    {
        public GroupInfo(
            int id,
            string name,
            CronExpression cyclingCronExpression,
            int dutiesCount,
            IReadOnlyCollection<GroupMemberInfo> currentDuties,
            IReadOnlyCollection<GroupMemberInfo> nextDuties,
            IReadOnlyCollection<RotationChangedTrigger> triggers)
        {
            Id = id;
            Name = name;
            CyclingCronExpression = cyclingCronExpression;
            DutiesCount = dutiesCount;
            CurrentDuties = currentDuties;
            NextDuties = nextDuties;
            Triggers = triggers;
        }

        public int Id { get; }

        public string Name { get; }
        
        public CronExpression CyclingCronExpression { get; }

        public int DutiesCount { get; }
        
        public IReadOnlyCollection<GroupMemberInfo> CurrentDuties { get; }
        
        public IReadOnlyCollection<GroupMemberInfo> NextDuties { get; }

        public IReadOnlyCollection<GroupMemberInfo> AllMembers => CurrentDuties.Concat(NextDuties).ToArray();
        
        // todo: introduce views for triggers to avoid trigger being ran outside of group
        public IReadOnlyCollection<RotationChangedTrigger> Triggers { get; }
    }
}