using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DutyCycle.Triggers;

namespace DutyCycle
{
    public interface IGroupService
    {
        Task<GroupInfo> GetGroup(int groupId);
        Task<IReadOnlyCollection<GroupInfo>> GetAllGroups();

        Task<Group> CreateGroup(GroupSettings groupSettings);
        Task EditGroup(int groupId, GroupSettings groupSettings);
        Task AddMemberToGroup(int groupId, NewGroupMemberInfo newGroupMemberInfo);

        Task AddTriggerOnRotationChange(int groupId, RotationChangedTrigger trigger);
        Task RemoveTrigger(int groupId, Guid callbackId);

        Task RotateDutiesInGroup(int groupId);
    }
}