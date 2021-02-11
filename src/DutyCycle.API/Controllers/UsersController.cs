using System;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.API.Authentication;
using DutyCycle.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutyCycle.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        public UsersController(IMapper mapper, IAuthenticationService authenticationService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authenticationService =
                authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }
        
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody]UserCredentials credentialsModel)
        {
            var credentials = _mapper.Map<Users.Domain.UserCredentials>(credentialsModel);
            
            await _authenticationService.SignIn(credentials, HttpContext);
            
            return Ok();
        }

        [HttpPost]
        [Route("signout")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOut(HttpContext);
            
            return Ok();
        }

        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
    }
}