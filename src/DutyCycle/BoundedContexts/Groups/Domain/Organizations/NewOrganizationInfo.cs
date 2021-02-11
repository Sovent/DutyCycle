using System;

namespace DutyCycle.Groups.Domain.Organizations
{
    public class NewOrganizationInfo
    {
        public NewOrganizationInfo(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }
    }
}