using System.Threading.Tasks;

namespace DutyCycle.Organizations
{
    public interface IOrganizationsService
    {
        Task<int> Create(NewOrganizationInfo newOrganizationInfo);
    }
}