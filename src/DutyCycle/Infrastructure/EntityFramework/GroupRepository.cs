using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace DutyCycle.Infrastructure.EntityFramework
{
    public class GroupRepository : IGroupRepository
    {
        public GroupRepository(DutyCycleDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Group> Get(int groupId)
        {
            var group = await GroupsWithRelatedData.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == default)
            {
                throw new GroupNotFound(groupId, DateTimeOffset.UtcNow).ToException();
            }

            return group;
        }

        public async Task<IReadOnlyCollection<Group>> GetForOrganization(int organizationId)
        {
            var groups = await GroupsWithRelatedData
                .Where(group => group.OrganizationId == organizationId)
                .ToListAsync();
            return groups;
        }

        public async Task Save(Group group)
        {
            if (group.Id == default)
            {
                await _dbContext.Groups.AddAsync(group);
            }

            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<Group> GroupsWithRelatedData =>
            _dbContext.Groups
                .Include("_groupMembers")
                .Include("_triggers");

        private readonly DutyCycleDbContext _dbContext;
    }
}