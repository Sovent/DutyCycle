using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;
using DutyCycle.Groups.Domain.Slack;

namespace DutyCycle.Groups.Application
{
    public class SlackConnectionService : ISlackIntegrationService
    {
        public SlackConnectionService(
            ISlackConnectionRepository slackConnectionRepository,
            IAddToSlackLinkProvider addToSlackLinkProvider,
            ISlackAccessTokenRetriever slackAccessTokenRetriever)
        {
            _slackConnectionRepository = slackConnectionRepository ??
                                         throw new ArgumentNullException(nameof(slackConnectionRepository));
            _addToSlackLinkProvider =
                addToSlackLinkProvider ?? throw new ArgumentNullException(nameof(addToSlackLinkProvider));
            _slackAccessTokenRetriever = slackAccessTokenRetriever ??
                                         throw new ArgumentNullException(nameof(slackAccessTokenRetriever));
        }
        
        public async Task<string> GetSlackConnectionLinkForOrganization(int organizationId)
        {
            var slackConnection = await _slackConnectionRepository.TryGetForOrganization(organizationId)
                .IfNoneAsync(
                    async () =>
                    {
                        var newConnection = SlackConnection.New(organizationId);
                        await _slackConnectionRepository.Create(newConnection);
                        return newConnection;
                    });
            var addToSlackLink = _addToSlackLinkProvider.GetLink(slackConnection.Id);
            return addToSlackLink;
        }

        public async Task CompleteSlackConnection(Guid connectionId, string authenticationCode)
        {
            var connection = await _slackConnectionRepository.GetById(connectionId);
            var accessToken = await _slackAccessTokenRetriever.RetrieveToken(authenticationCode);
            connection.SetAccessToken(accessToken);
            await _slackConnectionRepository.Update(connection);
        }

        private readonly ISlackConnectionRepository _slackConnectionRepository;
        private readonly IAddToSlackLinkProvider _addToSlackLinkProvider;
        private readonly ISlackAccessTokenRetriever _slackAccessTokenRetriever;
    }
}