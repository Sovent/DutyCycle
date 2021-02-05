using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("groups")]
    public class GroupsController : ControllerBase
    {
        public GroupsController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAllGroups();
            var groupModels = groups.Select(_mapper.Map<Models.Group>);
            return Ok(groupModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.GroupSettings request)
        {
            var groupSettings = _mapper.Map<GroupSettings>(request);
            // todo: get organization id from cookies
            var group = await _groupService.CreateGroup(1, groupSettings);
            return Ok(group.Id);
        }
        
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
    }
}