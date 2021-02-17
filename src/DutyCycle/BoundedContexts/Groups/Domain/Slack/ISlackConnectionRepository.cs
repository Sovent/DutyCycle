using System;
using System.Threading.Tasks;
using LanguageExt;

namespace DutyCycle.Groups.Domain.Slack
{
    public interface ISlackConnectionRepository
    {
        Task<SlackConnection> GetById(Guid id);

        Task<Option<SlackConnection>> TryGetForOrganization(int organizationId);

        Task Create(SlackConnection connection);

        Task Update(SlackConnection connection);
    }
}