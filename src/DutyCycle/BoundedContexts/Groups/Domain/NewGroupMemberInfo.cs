using System;

namespace DutyCycle.Groups.Domain
{
    public class NewGroupMemberInfo
    {
        public NewGroupMemberInfo(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Name = name;
        }

        public string Name { get; }
    }
}