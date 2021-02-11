using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DutyCycle.Common;
using DutyCycle.Users.Application;
using DutyCycle.Users.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace DutyCycle.API.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task SignUp(UserCredentials userCredentials, int organizationId, HttpContext httpContext)
        {
            await _userService.SignUpUser(userCredentials, organizationId);

            await SignIn(userCredentials, httpContext);
        }

        public async Task SignIn(UserCredentials userCredentials, HttpContext httpContext)
        {
            var foundUser = await _userService.FindByCredentials(userCredentials);
            var user = foundUser.GetOrThrow(() => new AuthenticationFailedException());

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(Constants.IdClaimType, user.Id.ToString()),
                new Claim(Constants.OrganizationIdClaimType, user.OrganizationId.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }

        public async Task SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<User> GetCurrentUser(HttpContext httpContext)
        {
            var idOption = httpContext.User.TryUserGetId();
            var userOption = await idOption.BindAsync(id => _userService.FindById(id).ToAsync()).ToOption();
            return userOption.GetOrThrow(() => new AuthenticationFailedException());
        }

        private readonly IUserService _userService;
    }
}