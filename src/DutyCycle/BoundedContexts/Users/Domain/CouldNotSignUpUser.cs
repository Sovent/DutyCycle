using System;
using System.Collections.Generic;
using DutyCycle.Common;

namespace DutyCycle.Users.Domain
{
    public class CouldNotSignUpUser : DomainError<CouldNotSignUpUser>
    {
        public CouldNotSignUpUser(IEnumerable<string> errors, DateTimeOffset occuredOnUtc) : base(occuredOnUtc)
        {
            Description = string.Join(Environment.NewLine, errors);
        }

        public override string Description { get; }
    }
}