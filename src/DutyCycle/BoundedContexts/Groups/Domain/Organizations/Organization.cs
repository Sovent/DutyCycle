using System;

namespace DutyCycle.Groups.Domain.Organizations
{
    public class Organization
    {
        public Organization(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            
            Name = name;
        }
        
        public int Id { get; private set; }
        
        public string Name { get; private set; }
    }
}