using System.Collections.Generic;

namespace DutyCycle.API.Models
{
    public class Group
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string CyclingCronExpression { get; set; }

        public int DutiesCount { get; set; }
        
        public IReadOnlyCollection<GroupMember> CurrentDuties { get; set; }
        
        public IReadOnlyCollection<GroupMember> NextDuties { get; set; }
        
        public IReadOnlyCollection<GroupActionTrigger> Triggers { get; set; }
    }
}