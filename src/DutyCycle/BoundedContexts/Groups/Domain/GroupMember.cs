using System;

namespace DutyCycle.Groups.Domain
{
    public class GroupMember
    {
        public GroupMember(Guid id, string name, Guid? followedGroupMemberId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            
            Id = id;
            Name = name;
            FollowedGroupMemberId = followedGroupMemberId;
        }

        public Guid Id { get; private set; }
        
        public string Name { get; private set; }
        
        public Guid? FollowedGroupMemberId { get; set; }
    }
}