using System;
using System.Threading.Tasks;
using DutyCycle.API.Authentication;
using DutyCycle.Groups.Application;
using DutyCycle.Users;
using DutyCycle.Users.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("groups/{groupId}/rotations")]
    [Authorize]
    public class RotationsController : ControllerBase
    {
        public RotationsController(IGroupService groupService, IUserPermissionsService userPermissionsService)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            _userPermissionsService =
                userPermissionsService ?? throw new ArgumentNullException(nameof(userPermissionsService));
        }
        
        [HttpPost]
        public async Task<IActionResult> ForceRotation(int groupId)
        {
            await _userPermissionsService.ValidateHasAccessToGroup(User.GetUserId(), groupId);
            
            await _groupService.RotateDutiesInGroup(groupId);
            return Ok();
        }
        
        private readonly IGroupService _groupService;
        private readonly IUserPermissionsService _userPermissionsService;
    }
}