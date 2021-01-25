using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cronos;
using DutyCycle.Triggers;

namespace DutyCycle
{
    public class Group
    {
        public Group(string name, CronExpression cyclingCronExpression, int dutiesCount)
        {
            Name = name;
            CyclingCronExpression = cyclingCronExpression;
            DutiesCount = dutiesCount;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }
        
        public CronExpression CyclingCronExpression { get; private set; }

        public int DutiesCount { get; private set; }

        public IEnumerable<GroupMember> Members
        {
            get
            {
                var result = new MembersOrderer().Order(Id, _groupMembers);
                return result.IfFail(exception => throw exception);
            }
        }

        public GroupInfo Info
        {
            get
            {
                var effectiveDutiesCount = Math.Min(DutiesCount, _groupMembers.Count);
                var memberViews = Members
                    .Select(member => new GroupMemberInfo(member.Id, member.Name, member.FollowedGroupMemberId))
                    .ToArray();
                var currentDuties = memberViews[..effectiveDutiesCount];
                var nextDuties = memberViews[effectiveDutiesCount..];
                return new GroupInfo(
                    Id,
                    Name, 
                    CyclingCronExpression, 
                    DutiesCount, 
                    currentDuties, 
                    nextDuties,
                    _triggers);
            }
        }

        public async Task RotateDuties(TriggersContext triggersContext)
        {
            var groupMembersCount = _groupMembers.Count;
            if (groupMembersCount == 0)
            {
                return;
            }
            
            var positionsToAdvance = DutiesCount % groupMembersCount;
            if (positionsToAdvance == 0)
            {
                return;
            }

            var currentRotation = Members.ToArray();
            var currentTail = currentRotation[groupMembersCount - 1];
            var currentFirst = currentRotation[0];
            var newFirst = currentRotation[positionsToAdvance];

            newFirst.FollowedGroupMemberId = null;
            currentFirst.FollowedGroupMemberId = currentTail.Id;

            await RunTriggers(GroupAction.RotateDuties, triggersContext);
        }

        public async Task AddMember(NewGroupMemberInfo newGroupMemberInfo, TriggersContext triggersContext)
        {
            var lastMemberId = Members.LastOrNone().Map(member => member.Id).ToNullable();
            var newGroupMember = new GroupMember(Guid.NewGuid(), newGroupMemberInfo.Name, lastMemberId);
            _groupMembers.Add(newGroupMember);

            await RunTriggers(GroupAction.AddMember, triggersContext);
        }

        public void AddActionTrigger(GroupActionTrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public void RemoveActionCallback(Guid triggerId)
        {
            _triggers.RemoveAll(trigger => trigger.Id == triggerId);
        }

        private async Task RunTriggers(GroupAction action, TriggersContext triggersContext)
        {
            var triggersToRun = _triggers
                .Where(actionTrigger => actionTrigger.Action == action)
                .Select(trigger => trigger.Run(Info, triggersContext));

            await Task.WhenAll(triggersToRun);
        }

        private List<GroupActionTrigger> _triggers = new List<GroupActionTrigger>();
        private List<GroupMember> _groupMembers = new List<GroupMember>();
    }
}