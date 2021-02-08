using System;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.API.Authentication;
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
            IAuthenticationService authenticationService)
        {
            _organizationsService =
                organizationsService ?? throw new ArgumentNullException(nameof(organizationsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authenticationService = 
                authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Models.NewOrganizationInfo newOrganizationRequest)
        {
            var newOrganizationInfo = _mapper.Map<NewOrganizationInfo>(newOrganizationRequest);
            var newOrganizationId = await _organizationsService.Create(newOrganizationInfo);

            var organizationUser = _mapper.Map<UserCredentials>(newOrganizationRequest.AdminCredentials);

            await _authenticationService.SignUp(organizationUser, newOrganizationId, HttpContext);

            return Ok(newOrganizationId);
        }
        
        private readonly IOrganizationsService _organizationsService;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
    }
}