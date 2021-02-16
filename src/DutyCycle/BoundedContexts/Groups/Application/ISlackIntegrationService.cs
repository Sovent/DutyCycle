using System;
using System.Threading.Tasks;

namespace DutyCycle.Groups.Application
{
    public interface ISlackIntegrationService
    {
        Task<string> GetSlackConnectionLinkForOrganization(int organizationId);

        Task CompleteSlackConnection(Guid connectionId, string authenticationCode);
    }
}