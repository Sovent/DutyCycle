using System;

namespace DutyCycle.Users.Domain
{
    public class UserCredentials
    {
        public UserCredentials(string email, string password)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string Email { get; }
        
        public string Password { get; }
    }
}