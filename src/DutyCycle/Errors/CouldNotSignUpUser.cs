using System;
using System.Collections.Generic;

namespace DutyCycle.Errors
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