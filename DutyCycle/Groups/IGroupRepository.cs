using System.Collections.Generic;
using System.Threading.Tasks;

namespace DutyCycle
{
    public interface IGroupRepository
    {
        Task<Group> Get(int groupId);

        Task<IReadOnlyCollection<Group>> GetAll();

        Task Save(Group group);
    }
}