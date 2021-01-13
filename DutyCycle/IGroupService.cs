using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;

namespace DutyCycle
{
    public interface IGroupService
    {
        Task<Option<Group>> TryGetGroup(int groupId);
        Task<IReadOnlyCollection<Group>> GetAllGroups();

        Task<Group> CreateGroup(GroupSettings groupSettings);
        Task AddMemberToGroup(int groupId, GroupMemberInfo groupMemberInfo);
    }
}