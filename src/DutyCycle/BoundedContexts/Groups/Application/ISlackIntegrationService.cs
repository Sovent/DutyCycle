using System.Threading.Tasks;

namespace DutyCycle.Groups.Application
{
    public interface ISlackIntegrationService
    {
        Task<string> GetSlackConnectionLinkForOrganization(int organizationId);
    }
}