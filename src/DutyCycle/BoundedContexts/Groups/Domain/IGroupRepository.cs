using System.Collections.Generic;
using System.Threading.Tasks;

namespace DutyCycle.Groups.Domain
{
    public interface IGroupRepository
    {
        Task<Group> Get(int groupId);

        Task<IReadOnlyCollection<Group>> GetForOrganization(int organizationId);

        Task Save(Group group);
    }
}