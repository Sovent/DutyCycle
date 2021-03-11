using System;

namespace DutyCycle.Groups.Domain.Organizations
{
    public class OrganizationSlackInfo
    {
        public OrganizationSlackInfo(string workspaceName)
        {
            WorkspaceName = workspaceName ?? throw new ArgumentNullException(nameof(workspaceName));
        }

        public string WorkspaceName { get; }
    }
}