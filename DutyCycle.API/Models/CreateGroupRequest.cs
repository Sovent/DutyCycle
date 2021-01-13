namespace DutyCycle.API.Models
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }
        
        public string CyclingCronExpression { get; set; }
        
        public int DutiesCount { get; set; }
    }
}