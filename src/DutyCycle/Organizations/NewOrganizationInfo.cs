using System;

namespace DutyCycle.Organizations
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