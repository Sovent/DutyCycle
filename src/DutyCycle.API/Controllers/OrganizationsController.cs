using System;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.Organizations;
using DutyCycle.Users;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("organizations")]
    public class OrganizationsController : ControllerBase
    {
        public OrganizationsController(
            IOrganizationsService organizationsService, 
            IMapper mapper,
            IUserService userService)
        {
            _organizationsService =
                organizationsService ?? throw new ArgumentNullException(nameof(organizationsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Models.NewOrganizationInfo newOrganizationRequest)
        {
            var newOrganizationInfo = _mapper.Map<NewOrganizationInfo>(newOrganizationRequest);
            var newOrganizationId = await _organizationsService.Create(newOrganizationInfo);

            var organizationUser = _mapper.Map<UserCredentials>(newOrganizationRequest.AdminCredentials);
            await _userService.SignUpUser(organizationUser, newOrganizationId);

            return Ok(newOrganizationId);
        }
        
        private readonly IOrganizationsService _organizationsService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
    }
}