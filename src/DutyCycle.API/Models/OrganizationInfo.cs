namespace DutyCycle.API.Models
{
    public class OrganizationInfo
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public OrganizationSlackInfo SlackInfo { get; set; }
    }
}