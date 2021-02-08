using Microsoft.AspNetCore.Identity;

namespace DutyCycle.Users
{
    public class User : IdentityUser<int>
    {
        public int OrganizationId { get; set; }
    }
}