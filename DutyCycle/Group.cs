using System;
using System.Collections.Generic;
using System.Linq;

namespace DutyCycle
{
    public class Group
    {
        public Group(string name, string cyclingCronExpression, int dutiesCount)
        {
            Name = name;
            CyclingCronExpression = cyclingCronExpression;
            DutiesCount = dutiesCount;
        }

        public int Id => _id;
        
        public string Name { get; private set; }
        
        public string CyclingCronExpression { get; private set; }

        public int DutiesCount { get; private set; }

        public IReadOnlyCollection<GroupMember> Members => _groupMembers;

        public IEnumerable<GroupMember> CurrentDuties
        {
            get
            {
                var effectiveDutiesCount = Math.Min(DutiesCount, _groupMembers.Count);
                return _groupMembers.Take(effectiveDutiesCount);
            }
        }

        public void AddMember(GroupMemberInfo groupMemberInfo)
        {
            var lastMemberId = _groupMembers.LastOrNone().Map(member => member.Id).ToNullable();
            var newGroupMember = new GroupMember(Guid.NewGuid(), groupMemberInfo.Name, lastMemberId);
            _groupMembers.Add(newGroupMember);
        }

        private int _id;
        private List<GroupMember> _groupMembers = new List<GroupMember>();
    }
}