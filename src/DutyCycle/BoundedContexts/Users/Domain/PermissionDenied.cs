using System;
using DutyCycle.Common;

namespace DutyCycle.Users.Domain
{
    public class PermissionDenied : DomainError<PermissionDenied>
    {
        public PermissionDenied(int userId, string action, string reason, DateTimeOffset occuredOnUtc) 
            : base(occuredOnUtc)
        {
            Description = $"User with id {userId} is not allowed to perform {action}. Reason: {reason}";
        }

        public override string Description { get; }
    }
}