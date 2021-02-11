using System;
using System.Linq;
using System.Threading.Tasks;
using DutyCycle.Users.Domain;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DutyCycle.Users.Application
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

        public async Task<Option<User>> FindByCredentials(UserCredentials credentials)
        {
            var userWithProvidedEmail = (Option<User>)await _userManager.FindByEmailAsync(credentials.Email);
            return await userWithProvidedEmail
                .FilterAsync(user => _userManager.CheckPasswordAsync(user, credentials.Password))
                .ToOption();
        }

        public async Task<Option<User>> FindById(int userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);
        }

        private readonly UserManager<User> _userManager;
    }
}