using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;

namespace DutyCycle.Groups.Application
{
    public class SlackIntegrationService : ISlackIntegrationService
    {
        public SlackIntegrationService(
            ISlackConnectionRepository slackConnectionRepository,
            IAddToSlackLinkProvider addToSlackLinkProvider)
        {
            _slackConnectionRepository = slackConnectionRepository ??
                                         throw new ArgumentNullException(nameof(slackConnectionRepository));
            _addToSlackLinkProvider =
                addToSlackLinkProvider ?? throw new ArgumentNullException(nameof(addToSlackLinkProvider));
        }
        
        public async Task<string> GetSlackConnectionLinkForOrganization(int organizationId)
        {
            var slackConnection = await _slackConnectionRepository.TryGetForOrganization(organizationId)
                .IfNoneAsync(
                    async () =>
                    {
                        var newConnection = SlackConnection.New(organizationId);
                        await _slackConnectionRepository.Save(newConnection);
                        return newConnection;
                    });
            var addToSlackLink = _addToSlackLinkProvider.GetLink(slackConnection.Id);
            return addToSlackLink;
        }

        private readonly ISlackConnectionRepository _slackConnectionRepository;
        private readonly IAddToSlackLinkProvider _addToSlackLinkProvider;
    }
}