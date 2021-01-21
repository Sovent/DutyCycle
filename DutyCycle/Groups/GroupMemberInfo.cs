using System;

namespace DutyCycle
{
    public class GroupMemberInfo
    {
        public GroupMemberInfo(Guid id, string name, Guid? followedGroupMemberId)
        {
            Id = id;
            Name = name;
            FollowedGroupMemberId = followedGroupMemberId;
        }

        public Guid Id { get; }
        
        public string Name { get; }
        
        public Guid? FollowedGroupMemberId { get; }
    }
}