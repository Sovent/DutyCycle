using System;

namespace DutyCycle.Errors
{
    public class OrganizationNotFound : DomainError<OrganizationNotFound>
    {
        public OrganizationNotFound(int id, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            Description = $"Organization with id = '{id}' not found";
        }

        public override string Description { get; }
    }
}