using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Slack;

namespace DutyCycle.Groups.Domain.Triggers
{
    public class TriggersContextFactory : ITriggersContextFactory
    {
        public TriggersContextFactory(
            ISlackConnectionRepository slackConnectionRepository,
            ISlackClientFactory slackClientFactory)
        {
            _slackConnectionRepository = slackConnectionRepository ??
                                         throw new ArgumentNullException(nameof(slackConnectionRepository));
            _slackClientFactory = slackClientFactory ?? throw new ArgumentNullException(nameof(slackClientFactory));
        }
        
        public async Task<TriggersContext> CreateContext(int organizationId)
        {
            var organizationSlackConnection = await _slackConnectionRepository.TryGetForOrganization(organizationId);
            var slackClient = _slackClientFactory.Create(organizationId, organizationSlackConnection);
            var slackMessageTemplater = new SlackMessageTemplater();
            return new TriggersContext(slackClient, slackMessageTemplater);
        }
        
        private readonly ISlackConnectionRepository _slackConnectionRepository;
        private readonly ISlackClientFactory _slackClientFactory;
    }
}