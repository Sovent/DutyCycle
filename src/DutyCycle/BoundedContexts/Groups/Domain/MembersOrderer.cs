using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;

namespace DutyCycle.Groups.Domain
{
    public class MembersOrderer : IMembersOrderer
    {
        public Result<IReadOnlyCollection<GroupMember>> Order(int groupId, IReadOnlyCollection<GroupMember> members)
        {
            Map<Guid, GroupMember> followedToFollowerMap;
            try
            {
                followedToFollowerMap = members
                    .ToDictionary(member => member.FollowedGroupMemberId ?? Guid.Empty)
                    .ToMap();
            }
            catch (ArgumentException)
            {
                var exception = new GroupMembersOrderIsBrokenException(groupId);
                return new Result<IReadOnlyCollection<GroupMember>>(exception);
            }

            List<GroupMember> CollectMembers(List<GroupMember> buffer, Guid currentQueuePosition)
            {
                return followedToFollowerMap
                    .Find(currentQueuePosition)
                    .Match(follower =>
                        {
                            buffer.Add(follower);
                            return CollectMembers(buffer, follower.Id);
                        },
                        () => buffer);
            }

            var orderedMembers = CollectMembers(new List<GroupMember>(), Guid.Empty);
            var isValid = members.Count() == orderedMembers.Count;
            if (!isValid)
            {
                var exception = new GroupMembersOrderIsBrokenException(groupId);
                return new Result<IReadOnlyCollection<GroupMember>>(exception);
            }

            return orderedMembers;
        }
    }
}