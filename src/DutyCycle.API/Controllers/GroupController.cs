using System;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.API.Authentication;
using Microsoft.AspNetCore.Mvc;
using DutyCycle.API.Models;
using DutyCycle.Users;
using Microsoft.AspNetCore.Authorization;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("groups/{groupId}")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        public GroupController(IGroupService groupService, IMapper mapper, IUserPermissionsService permissionsService)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _permissionsService = permissionsService ?? throw new ArgumentNullException(nameof(permissionsService));
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute]int groupId)
        {
            await _permissionsService.ValidateHasAccessToGroup(User.GetUserId(), groupId);
            
            var group = await _groupService.GetGroup(groupId);
            var groupModel = _mapper.Map<Models.Group>(group);
            return Ok(groupModel);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeGroupSettings(
            [FromRoute] int groupId,
            [FromBody] Models.GroupSettings request)
        {
            await _permissionsService.ValidateHasAccessToGroup(User.GetUserId(), groupId);
            
            var groupSettings = _mapper.Map<GroupSettings>(request);
            await _groupService.EditGroup(groupId, groupSettings);
            return Ok();
        }

        [HttpPost]
        [Route("members")]
        public async Task<IActionResult> AddMember([FromRoute]int groupId, [FromBody]NewMemberInfo request)
        {
            await _permissionsService.ValidateHasAccessToGroup(User.GetUserId(), groupId);
            
            var groupMemberInfo = new NewGroupMemberInfo(request.Name);
            await _groupService.AddMemberToGroup(groupId, groupMemberInfo);
            return Ok();
        }

        [HttpPost]
        [Route("triggers")]
        public async Task<IActionResult> AddTrigger(
            [FromRoute] int groupId,
            [FromBody] RotationChangedTrigger request)
        {
            await _permissionsService.ValidateHasAccessToGroup(User.GetUserId(), groupId);
            
            var trigger = _mapper.Map<Triggers.RotationChangedTrigger>(request);
            await _groupService.AddTriggerOnRotationChange(groupId, trigger);
            return Ok();
        }

        [HttpDelete]
        [Route("triggers/{triggerId}")]
        public async Task<IActionResult> RemoveTrigger([FromRoute] int groupId, [FromRoute] Guid triggerId)
        {
            await _permissionsService.ValidateHasAccessToGroup(User.GetUserId(), groupId);
            
            await _groupService.RemoveTrigger(groupId, triggerId);
            return Ok();
        }
        
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        private readonly IUserPermissionsService _permissionsService;
    }
}