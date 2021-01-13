using System;

namespace DutyCycle.Errors
{
    public class GroupNotFound : DomainError<GroupNotFound>
    {
        public GroupNotFound(int groupId, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            GroupId = groupId;
        }
        
        public int GroupId { get; }
        
        public override string Description => $"Group with id '{GroupId}' not found";
    }
}