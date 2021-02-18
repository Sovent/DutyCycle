using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;

namespace DutyCycle.Groups.Application
{
    public interface IOrganizationsService
    {
        Task<int> Create(NewOrganizationInfo newOrganizationInfo);

        Task<Organization> GetOrganizationInfo(int organizationId);
    }
}