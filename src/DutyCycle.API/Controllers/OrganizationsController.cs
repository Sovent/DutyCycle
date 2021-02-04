using System;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.Organizations;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("organizations")]
    public class OrganizationsController : ControllerBase
    {
        public OrganizationsController(IOrganizationsService organizationsService, IMapper mapper)
        {
            _organizationsService =
                organizationsService ?? throw new ArgumentNullException(nameof(organizationsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Models.NewOrganizationInfo newOrganizationRequest)
        {
            var newOrganizationInfo = _mapper.Map<NewOrganizationInfo>(newOrganizationRequest);
            var newOrganizationId = await _organizationsService.Create(newOrganizationInfo);
            return Ok(newOrganizationId);
        }
        
        private readonly IOrganizationsService _organizationsService;
        private readonly IMapper _mapper;
    }
}