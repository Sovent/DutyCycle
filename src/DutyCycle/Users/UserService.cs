using System;
using System.Linq;
using System.Threading.Tasks;
using DutyCycle.Errors;
using Microsoft.AspNetCore.Identity;

namespace DutyCycle.Users
{
    public class UserService : IUserService
    {
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        
        public async Task<int> SignUpUser(UserCredentials credentials, int organizationId)
        {
            var newUser = new User
            {
                Email = credentials.Email,
                UserName = credentials.Email,
                OrganizationId = organizationId
            };

            var result = await _userManager.CreateAsync(newUser, credentials.Password);
            if (result.Succeeded)
            {
                return newUser.Id;
            }

            throw new CouldNotSignUpUser(
                    result.Errors.Select(error => error.Description), 
                    DateTimeOffset.UtcNow)
                .ToException();
        }
        
        private readonly UserManager<User> _userManager;
    }
}