using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DutyCycle.API.Models;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("groups/{groupId}")]
    public class GroupController : ControllerBase
    {
        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute]int groupId)
        {
            var group = await _groupService.GetGroup(groupId);
            var groupModel = _mapper.Map<Models.Group>(group);
            return Ok(groupModel);
        }

        [HttpPost]
        [Route("members")]
        public async Task<IActionResult> AddMember([FromRoute]int groupId, [FromBody]AddMemberRequest request)
        {
            var groupMemberInfo = new NewGroupMemberInfo(request.Name);
            await _groupService.AddMemberToGroup(groupId, groupMemberInfo);
            return Ok();
        }

        [HttpPost]
        [Route("callbacks")]
        public async Task<IActionResult> AddCallback(
            [FromRoute] int groupId,
            [FromBody] SendSlackMessageTrigger request) // todo: polymorphic deserialization of add trigger
        {
            var callback = _mapper.Map<Triggers.GroupActionTrigger>(request);
            await _groupService.AddCallback(groupId, callback);
            return Ok();
        }

        [HttpDelete]
        [Route("callbacks/{callbackId}")]
        public async Task<IActionResult> RemoveCallback([FromRoute] int groupId, [FromRoute] Guid callbackId)
        {
            await _groupService.RemoveCallback(groupId, callbackId);
            return Ok();
        }
        
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
    }
}