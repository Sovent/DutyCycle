using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutyCycle.Triggers;

namespace DutyCycle
{
    public class Group
    {
        public Group(string name, string cyclingCronExpression, int dutiesCount)
        {
            if (dutiesCount <= 0) throw new ArgumentOutOfRangeException(nameof(dutiesCount));
            
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CyclingCronExpression = cyclingCronExpression 
                                    ?? throw new ArgumentNullException(nameof(cyclingCronExpression));
            DutiesCount = dutiesCount;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }
        
        public string CyclingCronExpression { get; private set; }

        public int DutiesCount { get; private set; }

        public IReadOnlyCollection<GroupMember> Members
        {
            get
            {
                var result = new MembersOrderer().Order(Id, _groupMembers);
                return result.IfFail(exception => throw exception);
            }
        }

        public IEnumerable<GroupMember> CurrentDuties
        {
            get
            {
                var effectiveDutiesCount = Math.Min(DutiesCount, _groupMembers.Count);
                return Members.Take(effectiveDutiesCount);
            }
        }

        public async Task RotateDuties(TriggersTooling triggersTooling)
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

            await RunTrigger(GroupAction.RotateDuties, triggersTooling);
        }

        public async Task AddMember(GroupMemberInfo groupMemberInfo, TriggersTooling triggersTooling)
        {
            var lastMemberId = Members.LastOrNone().Map(member => member.Id).ToNullable();
            var newGroupMember = new GroupMember(Guid.NewGuid(), groupMemberInfo.Name, lastMemberId);
            _groupMembers.Add(newGroupMember);

            await RunTrigger(GroupAction.AddMember, triggersTooling);
        }

        public void AddActionCallback(GroupAction action, TriggerCallback callback)
        {
            var trigger = GetOrCreateTrigger(action);
            trigger.AddCallback(callback);
        }

        public void RemoveActionCallback(Guid callbackId)
        {
            foreach (var groupActionTrigger in _triggers)
            {
                groupActionTrigger.TryRemoveCallback(callbackId);
            }
        }

        private async Task RunTrigger(GroupAction action, TriggersTooling triggersTooling)
        {
            var trigger = GetOrCreateTrigger(action);
            await trigger.Run(this, triggersTooling);
        }
        
        private GroupActionTrigger GetOrCreateTrigger(GroupAction groupAction)
        {
            var trigger = _triggers.SingleOrDefault(action => action.Action == groupAction);
            if (trigger != default)
            {
                return trigger;
            }

            var triggerToCreate = new GroupActionTrigger(groupAction);
            _triggers.Add(triggerToCreate);
            return triggerToCreate;
        }

        private List<GroupActionTrigger> _triggers = new List<GroupActionTrigger>();
        private List<GroupMember> _groupMembers = new List<GroupMember>();
    }
}