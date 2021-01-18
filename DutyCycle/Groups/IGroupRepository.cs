using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;

namespace DutyCycle
{
    public interface IGroupRepository
    {
        Task<Option<Group>> TryGet(int groupId);

        Task<Group> Get(int groupId);

        Task<IReadOnlyCollection<Group>> GetAll();

        Task Save(Group group);
    }
}