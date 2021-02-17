using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cronos;
using DutyCycle.Groups.Domain;
using DutyCycle.Groups.Domain.Triggers;

namespace DutyCycle.Groups.Application
{
    public class GroupService : IGroupService
    {
        public GroupService(
            IGroupRepository repository,
            IGroupSettingsValidator groupSettingsValidator,
            IRotationScheduler rotationScheduler,
            ITriggersContextFactory triggersContextFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _groupSettingsValidator =
                groupSettingsValidator ?? throw new ArgumentNullException(nameof(groupSettingsValidator));
            _rotationScheduler = rotationScheduler ?? throw new ArgumentNullException(nameof(rotationScheduler));
            _triggersContextFactory =
                triggersContextFactory ?? throw new ArgumentNullException(nameof(triggersContextFactory));
        }
        
        public async Task<GroupInfo> GetGroup(int groupId)
        {
            var group = await _repository.Get(groupId);
            return group.Info;
        }

        public async Task<IReadOnlyCollection<GroupInfo>> GetGroupsForOrganization(int organizationId)
        {
            var groups = await _repository.GetForOrganization(organizationId);
            return groups.Select(group => group.Info).ToArray();
        }

        public async Task<Group> CreateGroup(int organizationId, GroupSettings groupSettings)
        {
            if (groupSettings == null) throw new ArgumentNullException(nameof(groupSettings));
            
            _groupSettingsValidator.Validate(groupSettings);

            var group = new Group(
                organizationId,
                groupSettings.Name, 
                CronExpression.Parse(groupSettings.CyclingCronExpression),
                groupSettings.DutiesCount);
            
            await _repository.Save(group);
            
            _rotationScheduler.ScheduleOrRescheduleForAGroup(group.Info);
            
            return group;
        }

        public async Task EditGroup(int groupId, GroupSettings groupSettings)
        {
            if (groupSettings == null) throw new ArgumentNullException(nameof(groupSettings));

            var group = await _repository.Get(groupId);
            
            _groupSettingsValidator.Validate(groupSettings);

            var triggersContext = await _triggersContextFactory.CreateContext(group.OrganizationId);
            
            await group.ChangeSettings(groupSettings, triggersContext);

            await _repository.Save(group);
            
            _rotationScheduler.ScheduleOrRescheduleForAGroup(group.Info);
        }

        public async Task AddMemberToGroup(int groupId, NewGroupMemberInfo newGroupMemberInfo)
        {
            if (newGroupMemberInfo == null) throw new ArgumentNullException(nameof(newGroupMemberInfo));
            
            var group = await _repository.Get(groupId);
            var triggersContext = await _triggersContextFactory.CreateContext(group.OrganizationId);
            await group.AddMember(newGroupMemberInfo, triggersContext);
            await _repository.Save(group);
        }

        public async Task AddTriggerOnRotationChange(int groupId, RotationChangedTrigger trigger)
        {
            if (trigger == null) throw new ArgumentNullException(nameof(trigger));
            
            var group = await _repository.Get(groupId);
            group.AddRotationChangedTrigger(trigger);
            await _repository.Save(group);
        }

        public async Task RemoveTrigger(int groupId, Guid triggerId)
        {
            var group = await _repository.Get(groupId);
            group.RemoveTrigger(triggerId);
            await _repository.Save(group);
        }

        public async Task RotateDutiesInGroup(int groupId)
        {
            var group = await _repository.Get(groupId);
            var triggersContext = await _triggersContextFactory.CreateContext(group.OrganizationId);
            await group.RotateDuties(triggersContext);
            await _repository.Save(group);
        }

        private readonly IGroupRepository _repository;
        private readonly IGroupSettingsValidator _groupSettingsValidator;
        private readonly IRotationScheduler _rotationScheduler;
        private readonly ITriggersContextFactory _triggersContextFactory;
    }
}