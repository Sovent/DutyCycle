using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain;
using DutyCycle.Groups.Domain.Triggers;

namespace DutyCycle.Groups.Application
{
    public interface IGroupService
    {
        Task<GroupInfo> GetGroup(int groupId);
        Task<IReadOnlyCollection<GroupInfo>> GetGroupsForOrganization(int organizationId);

        Task<Group> CreateGroup(int organizationId, GroupSettings groupSettings);
        Task EditGroup(int groupId, GroupSettings groupSettings);
        Task AddMemberToGroup(int groupId, NewGroupMemberInfo newGroupMemberInfo);

        Task AddTriggerOnRotationChange(int groupId, RotationChangedTrigger trigger);
        Task RemoveTrigger(int groupId, Guid triggerId);

        Task RotateDutiesInGroup(int groupId);
    }
}