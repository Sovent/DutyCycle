using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace DutyCycle.Infrastructure.EntityFramework
{
    public class SlackConnectionRepository : ISlackConnectionRepository
    {
        public SlackConnectionRepository(DutyCycleDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public Task<SlackConnection> GetById(Guid id)
        {
            var connection = _dbContext.SlackConnections.FirstOrDefaultAsync(c => c.Id == id);
            if (connection == default)
            {
                throw new SlackConnectionNotFound(id, DateTimeOffset.UtcNow).ToException();
            }

            return connection;
        }

        public async Task<Option<SlackConnection>> TryGetForOrganization(int organizationId)
        {
            return await _dbContext.SlackConnections
                .FirstOrDefaultAsync(connection => connection.OrganizationId == organizationId);
        }

        public async Task Create(SlackConnection connection)
        {
            await _dbContext.SlackConnections.AddAsync(connection);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(SlackConnection connection)
        {
            await _dbContext.SaveChangesAsync();
        }

        private readonly DutyCycleDbContext _dbContext;
    }
}