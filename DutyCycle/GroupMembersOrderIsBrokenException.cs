using System;
using System.Runtime.Serialization;

namespace DutyCycle
{
    [Serializable]
    public class GroupMembersOrderIsBrokenException : Exception
    {
        public GroupMembersOrderIsBrokenException(int groupId) 
            : base($"Members order in group {groupId} is not sequential, " +
                   $"check {nameof(GroupMember.FollowedGroupMemberId)} of each")
        {
            GroupId = groupId;
        }
        
        public GroupMembersOrderIsBrokenException()
        {
        }

        public GroupMembersOrderIsBrokenException(string message) : base(message)
        {
        }

        public GroupMembersOrderIsBrokenException(string message, Exception inner) : base(message, inner)
        {
        }

        public int GroupId { get; }
        
        protected GroupMembersOrderIsBrokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}