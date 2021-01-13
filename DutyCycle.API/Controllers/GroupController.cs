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
        public async Task<IActionResult> TryGet([FromRoute]int groupId)
        {
            var group = await _groupService.TryGetGroup(groupId);
            return group
                .Map(_mapper.Map<Models.Group>)
                .Match(
                    groupModel => (IActionResult) Ok(groupModel), 
                    NotFound);
        }

        [HttpPost]
        [Route("members")]
        public async Task<IActionResult> AddMember([FromRoute]int groupId, [FromBody]AddMemberRequest request)
        {
            var groupMemberInfo = new GroupMemberInfo(request.Name);
            await _groupService.AddMemberToGroup(groupId, groupMemberInfo);
            return Ok();
        }
        
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
    }
}