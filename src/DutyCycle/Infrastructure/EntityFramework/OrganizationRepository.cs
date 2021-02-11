using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain;
using DutyCycle.Groups.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace DutyCycle.Infrastructure.EntityFramework
{
    public class OrganizationRepository : IOrganizationRepository
    {
        public OrganizationRepository(DutyCycleDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<Organization> Get(int id)
        {
            var organization = await _dbContext.Organizations.FirstOrDefaultAsync(o => o.Id == id);
            if (organization == default)
            {
                throw new OrganizationNotFound(id, DateTimeOffset.UtcNow).ToException();
            }

            return organization;
        }

        public async Task Save(Organization organization)
        {
            if (organization.Id == default)
            {
                await _dbContext.Organizations.AddAsync(organization);
            }

            await _dbContext.SaveChangesAsync();
        }
        
        private readonly DutyCycleDbContext _dbContext;
    }
}