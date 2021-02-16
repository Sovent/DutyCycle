using System;
using System.Threading.Tasks;
using DutyCycle.API.Authentication;
using DutyCycle.Groups.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    public class SlackController : ControllerBase
    {
        public SlackController(ISlackIntegrationService slackIntegrationService)
        {
            _slackIntegrationService = slackIntegrationService ??
                                       throw new ArgumentNullException(nameof(slackIntegrationService));
        }

        [Authorize]
        [HttpGet]
        [Route("addtoslacklink")]
        public async Task<IActionResult> GetAddToSlackLink()
        {
            var authenticatedOrganizationId = User.GetOrganizationId();

            var link =
                await _slackIntegrationService.GetSlackConnectionLinkForOrganization(authenticatedOrganizationId);
            
            return Ok(link);
        }
        
        private readonly ISlackIntegrationService _slackIntegrationService;
    }
}