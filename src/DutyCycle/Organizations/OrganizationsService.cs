using System;
using System.Threading.Tasks;

namespace DutyCycle.Organizations
{
    public class OrganizationsService : IOrganizationsService
    {
        public OrganizationsService(IOrganizationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<int> Create(NewOrganizationInfo newOrganizationInfo)
        {
            if (newOrganizationInfo == null) throw new ArgumentNullException(nameof(newOrganizationInfo));

            var organizationToCreate = new Organization(newOrganizationInfo.Name);

            await _repository.Save(organizationToCreate);

            return organizationToCreate.Id;
        }
        
        private readonly IOrganizationRepository _repository;
    }
}