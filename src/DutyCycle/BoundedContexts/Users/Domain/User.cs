using Microsoft.AspNetCore.Identity;

namespace DutyCycle.Users.Domain
{
    public class User : IdentityUser<int>
    {
        public int OrganizationId { get; set; }
    }
}