using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DutyCycle.Errors;
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

        public async Task<Option<Group>> TryGet(int groupId)
        {
            return await _dbContext.Groups
                .Include("_groupMembers")
                .FirstOrDefaultAsync(group => group.Id == groupId);
        }

        public async Task<Group> Get(int groupId)
        {
            var group = await TryGet(groupId);
            return group.IfNone(() => throw new GroupNotFound(groupId, DateTimeOffset.UtcNow).ToException());
        }

        public async Task<IReadOnlyCollection<Group>> GetAll()
        {
            var groups = await _dbContext.Groups.ToListAsync();
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

        private readonly DutyCycleDbContext _dbContext;
    }
}