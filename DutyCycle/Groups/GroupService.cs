using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DutyCycle.Triggers;
using LanguageExt;

namespace DutyCycle
{
    public class GroupService : IGroupService
    {
        public GroupService(IGroupRepository repository, TriggersTooling triggersTooling)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _triggersTooling = triggersTooling ?? throw new ArgumentNullException(nameof(triggersTooling));
        }
        
        public Task<Option<Group>> TryGetGroup(int groupId)
        {
            return _repository.TryGet(groupId);
        }

        public Task<IReadOnlyCollection<Group>> GetAllGroups()
        {
            return _repository.GetAll();
        }

        public async Task<Group> CreateGroup(GroupSettings groupSettings)
        {
            if (groupSettings == null) throw new ArgumentNullException(nameof(groupSettings));
            
            var group = new Group(groupSettings.Name, groupSettings.CyclingCronExpression, groupSettings.DutiesCount);
            await _repository.Save(group);
            return group;
        }

        public async Task AddMemberToGroup(int groupId, GroupMemberInfo groupMemberInfo)
        {
            if (groupMemberInfo == null) throw new ArgumentNullException(nameof(groupMemberInfo));
            
            var group = await _repository.Get(groupId);
            await group.AddMember(groupMemberInfo, _triggersTooling);
            await _repository.Save(group);
        }

        public async Task RotateDutiesInGroup(int groupId)
        {
            var group = await _repository.Get(groupId);
            await group.RotateDuties(_triggersTooling);
            await _repository.Save(group);
        }

        private readonly IGroupRepository _repository;
        private readonly TriggersTooling _triggersTooling;
    }
}