using System;

namespace DutyCycle.Groups.Domain
{
    public class GroupSettings
    {
        public GroupSettings(string name, string cyclingCronExpression, int dutiesCount)
        {
            if (dutiesCount <= 0) throw new ArgumentOutOfRangeException(nameof(dutiesCount));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CyclingCronExpression =
                cyclingCronExpression ?? throw new ArgumentNullException(nameof(cyclingCronExpression));
            DutiesCount = dutiesCount;
        }

        public string Name { get; }
        
        public string CyclingCronExpression { get; }
        
        public int DutiesCount { get; }
    }
}