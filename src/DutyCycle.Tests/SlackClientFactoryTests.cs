using DutyCycle.Groups.Domain.Slack;
using DutyCycle.Infrastructure.Slack;
using LanguageExt;
using NUnit.Framework;

namespace DutyCycle.Tests
{
    public class SlackClientFactoryTests
    {
        [Test]
        public void CreateClientNoSlackConnectionProvided_ReturnsNoConnectionClient()
        {
            var slackClientFactory = new SlackClientFactory();

            var client = slackClientFactory.Create(1, Option<SlackConnection>.None);
            
            Assert.IsInstanceOf<NotConnectedSlackClient>(client);
        }
        
        [Test]
        public void CreateClientWithNotFinishedConnection_ReturnsNoConnectionClient()
        {
            var slackClientFactory = new SlackClientFactory();
            const int organizationId = 1;
            
            var client = slackClientFactory.Create(organizationId, SlackConnection.New(organizationId));
            
            Assert.IsInstanceOf<NotConnectedSlackClient>(client);
        }
        
        [Test]
        public void CreateClientWithFinishedConnection_ReturnsWorkingSlackClient()
        {
            var slackClientFactory = new SlackClientFactory();
            const int organizationId = 1;
            var slackConnection = SlackConnection.New(organizationId);
            slackConnection.SetAccessToken("some token");
            
            var client = slackClientFactory.Create(organizationId, slackConnection);
            
            Assert.IsInstanceOf<SlackClient>(client);
        }
    }
}