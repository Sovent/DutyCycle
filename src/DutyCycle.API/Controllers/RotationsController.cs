using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("groups/{groupId}/rotations")]
    public class RotationsController : ControllerBase
    {
        public RotationsController(IGroupService groupService)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }
        
        [HttpPost]
        public async Task<IActionResult> ForceRotation(int groupId)
        {
            await _groupService.RotateDutiesInGroup(groupId);
            return Ok();
        }
        
        private readonly IGroupService _groupService;
    }
}