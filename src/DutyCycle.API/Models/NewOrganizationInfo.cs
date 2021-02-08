using System.ComponentModel.DataAnnotations;

namespace DutyCycle.API.Models
{
    public class NewOrganizationInfo
    {
        public string Name { get; set; }
        
        [Required]
        public UserCredentials AdminCredentials { get; set; }
    }
}