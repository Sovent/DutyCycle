using System;

namespace DutyCycle.API.Models
{
    public class GroupMember
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public Guid? FollowedGroupMemberId { get; set; }
    }
}