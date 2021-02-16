using System;

namespace DutyCycle.Groups.Domain.Organizations
{
    public class SlackConnection
    {
        public static SlackConnection New(int organizationId) =>
            new SlackConnection(Guid.NewGuid(), organizationId);

        private SlackConnection(Guid id, int organizationId)
        {
            Id = id;
            OrganizationId = organizationId;
        }

        public Guid Id { get; private set; }
        
        public int OrganizationId { get; private set; }
    }
}