using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;
using DutyCycle.Groups.Domain.Slack;

namespace DutyCycle.Groups.Application
{
    public class OrganizationsService : IOrganizationsService
    {
        public OrganizationsService(
            IOrganizationRepository repository,
            ISlackConnectionRepository slackConnectionRepository,
            ISlackClientFactory slackClientFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _slackConnectionRepository = slackConnectionRepository ??
                                         throw new ArgumentNullException(nameof(slackConnectionRepository));
            _slackClientFactory = slackClientFactory ?? throw new ArgumentNullException(nameof(slackClientFactory));
        }
        
        public async Task<int> Create(NewOrganizationInfo newOrganizationInfo)
        {
            if (newOrganizationInfo == null) throw new ArgumentNullException(nameof(newOrganizationInfo));

            var organizationToCreate = new Organization(newOrganizationInfo.Name);

            await _repository.Save(organizationToCreate);

            return organizationToCreate.Id;
        }

        public async Task<OrganizationInfo> GetOrganizationInfo(int organizationId)
        {
            var organization = await _repository.Get(organizationId);
            var slackConnection = await _slackConnectionRepository.TryGetForOrganization(organizationId);
            var slackInfo = await slackConnection
                .Where(connection => connection.IsComplete)
                .MapAsync(connection =>
                {
                    var slackClient = _slackClientFactory.Create(organizationId, connection);
                    return slackClient.GetInfo();
                }).ToOption();
            
            return new OrganizationInfo(organization.Id, organization.Name, slackInfo);
        }

        private readonly IOrganizationRepository _repository;
        private readonly ISlackConnectionRepository _slackConnectionRepository;
        private readonly ISlackClientFactory _slackClientFactory;
    }
}