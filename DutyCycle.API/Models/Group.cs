using System.Collections.Generic;

namespace DutyCycle.API.Models
{
    public class Group
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string CyclingCronExpression { get; set; }

        public int DutiesCount { get; set; }
        
        public IReadOnlyCollection<GroupMember> Members { get; set; }
    }
}