using System;

namespace DutyCycle.Errors
{
    public class InvalidGroupSettings : DomainError<InvalidGroupSettings>
    {
        public InvalidGroupSettings(string description, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            Description = description;
        }

        public override string Description { get; }
    }
}