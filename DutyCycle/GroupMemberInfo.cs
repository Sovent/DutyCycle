using System;

namespace DutyCycle
{
    public class GroupMemberInfo
    {
        public GroupMemberInfo(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Name = name;
        }

        public string Name { get; }
    }
}