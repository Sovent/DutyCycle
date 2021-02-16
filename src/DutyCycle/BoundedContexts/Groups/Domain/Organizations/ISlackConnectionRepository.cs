using System;
using System.Threading.Tasks;
using LanguageExt;

namespace DutyCycle.Groups.Domain.Organizations
{
    public interface ISlackConnectionRepository
    {
        Task<SlackConnection> GetById(Guid id);

        Task<Option<SlackConnection>> TryGetForOrganization(int organizationId);

        Task Save(SlackConnection connection);
    }
}