using System.Threading.Tasks;

namespace DutyCycle.Organizations
{
    public interface IOrganizationRepository
    {
        Task<Organization> Get(int id);

        Task Save(Organization organization);
    }
}