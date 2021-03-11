using System;
using System.Threading.Tasks;
using DutyCycle.API.Authentication;
using DutyCycle.API.Models;
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

        [HttpPost]
        [Route("slackconnection")]
        public async Task<IActionResult> ConfirmSlackConnection(
            [FromBody]SlackConnectionConfirmation confirmation)
        {
            await _slackIntegrationService.CompleteSlackConnection(
                confirmation.ConnectionId,
                confirmation.AuthenticationCode);

            return Ok();
        }
        
        private readonly ISlackIntegrationService _slackIntegrationService;
    }
}