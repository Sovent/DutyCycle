using System;
using System.Threading.Tasks;
using DutyCycle.Groups.Application;
using DutyCycle.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DutyCycle.Users.Application
{
    public class UserPermissionsService : IUserPermissionsService
    {
        public UserPermissionsService(UserManager<User> userManager, IGroupService groupService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }
        
        public async Task ValidateHasAccessToGroup(int userId, int groupId)
        {
            const string groupOperationActionDescription = "group operations";
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == default)
            {
                throw new PermissionDenied(
                    userId, 
                    groupOperationActionDescription, 
                    "user not found",
                    DateTimeOffset.UtcNow).ToException();
            }

            var group = await _groupService.GetGroup(groupId);
            if (group.OrganizationId != user.OrganizationId)
            {
                throw new PermissionDenied(
                    userId, 
                    groupOperationActionDescription, 
                    "user and group are not in the same organization",
                    DateTimeOffset.UtcNow).ToException();
            }
        }
        
        private readonly UserManager<User> _userManager;
        private readonly IGroupService _groupService;
    }
}