using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DutyCycle.Triggers;
using LanguageExt;

namespace DutyCycle
{
    public interface IGroupService
    {
        Task<Option<GroupInfo>> TryGetGroup(int groupId);
        Task<IReadOnlyCollection<GroupInfo>> GetAllGroups();

        Task<Group> CreateGroup(GroupSettings groupSettings);
        Task AddMemberToGroup(int groupId, NewGroupMemberInfo newGroupMemberInfo);

        Task AddCallback(int groupId, GroupActionTrigger trigger);
        Task RemoveCallback(int groupId, Guid callbackId);

        Task RotateDutiesInGroup(int groupId);
    }
}