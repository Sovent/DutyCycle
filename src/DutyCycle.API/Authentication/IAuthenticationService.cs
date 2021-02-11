using System.Threading.Tasks;
using DutyCycle.Users.Domain;
using Microsoft.AspNetCore.Http;

namespace DutyCycle.API.Authentication
{
    public interface IAuthenticationService
    {
        Task SignUp(UserCredentials userCredentials, int organizationId, HttpContext httpContext);
        
        Task SignIn(UserCredentials userCredentials, HttpContext httpContext);

        Task SignOut(HttpContext httpContext);

        Task<User> GetCurrentUser(HttpContext httpContext);
    }
}