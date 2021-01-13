using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;

namespace DutyCycle
{
    public class GroupService : IGroupService
    {
        public GroupService(IGroupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
            var group = new Group(groupSettings.Name, groupSettings.CyclingCronExpression, groupSettings.DutiesCount);
            await _repository.Save(group);
            return group;
        }

        public async Task AddMemberToGroup(int groupId, GroupMemberInfo groupMemberInfo)
        {
            var group = await _repository.Get(groupId);
            group.AddMember(groupMemberInfo);
            await _repository.Save(group);
        }
        
        private readonly IGroupRepository _repository;
    }
}