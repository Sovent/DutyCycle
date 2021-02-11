using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.API.Authentication;
using DutyCycle.Groups.Application;
using DutyCycle.Groups.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("groups")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        public GroupsController(
            IGroupService groupService,
            IMapper mapper,
            IAuthenticationService authenticationService)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authenticationService =
                authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUserOrganizationId = User.GetOrganizationId();
            
            var groups = await _groupService.GetGroupsForOrganization(currentUserOrganizationId);
            var groupModels = groups.Select(_mapper.Map<Models.Group>);
            return Ok(groupModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.GroupSettings request)
        {
            var currentUserOrganizationId = User.GetOrganizationId();
            
            var groupSettings = _mapper.Map<GroupSettings>(request);
            var group = await _groupService.CreateGroup(currentUserOrganizationId, groupSettings);
            return Ok(group.Id);
        }
        
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
    }
}