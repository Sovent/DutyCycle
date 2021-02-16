using System;
using DutyCycle.Groups.Domain.Organizations;

namespace DutyCycle.Infrastructure.Slack
{
    public class AddToSlackLinkProvider : IAddToSlackLinkProvider
    {
        public AddToSlackLinkProvider(string clientId)
        {
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        }
        
        public string GetLink(Guid slackConnectionIdentifier)
        {
            var link = "https://slack.com/oauth/v2/authorize?" +
                       $"scope={_requiredScopeString}&client_id={_clientId}&state={slackConnectionIdentifier}";
            return link;
        }

        private static readonly string _requiredScopeString = string.Join(
            ",", 
            "commands", 
            "chat:write",
            "chat:write.customize", 
            "chat:write.public", 
            "usergroups:read", 
            "usergroups:write");
        
        private readonly string _clientId;
    }
}