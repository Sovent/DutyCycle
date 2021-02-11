using System;
using DutyCycle.Common;

namespace DutyCycle.Groups.Domain
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