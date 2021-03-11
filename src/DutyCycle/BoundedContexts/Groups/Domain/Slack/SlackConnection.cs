using System;
using LanguageExt;

namespace DutyCycle.Groups.Domain.Slack
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

        public bool IsComplete => AccessToken.IsSome;
        
        public Option<string> AccessToken => _accessToken;

        public void SetAccessToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Access token cannot be null or whitespace.", nameof(token));
            
            _accessToken = token;
        }
        
        private string _accessToken;
    }
}