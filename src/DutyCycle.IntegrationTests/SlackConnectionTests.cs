using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using AutoFixture;
using DutyCycle.API.Models;
using DutyCycle.Groups.Domain.Slack;
using LanguageExt;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OrganizationInfo = DutyCycle.API.Models.OrganizationInfo;
using OrganizationSlackInfo = DutyCycle.Groups.Domain.Organizations.OrganizationSlackInfo;

namespace DutyCycle.IntegrationTests
{
    public class SlackConnectionTests : IntegrationTests
    {
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(_ => _tokenRetrieverMock.Object);
            serviceCollection.AddScoped(_ => _slackClientFactoryMock.Object);
        }
        
        [SetUp]
        public async Task Setup()
        {
            await CreateOrganizationAndAssertSuccess();
            
            _tokenRetrieverMock
                .Setup(retriever => retriever.RetrieveToken(It.IsAny<string>()))
                .Returns(Task.FromResult(SlackAccessToken));
            _slackClientFactoryMock
                .Setup(factory => factory.Create(It.IsAny<int>(), It.IsAny<Option<SlackConnection>>()))
                .Returns(_slackClientMock.Object);
            _slackClientMock
                .Setup(client => client.SendMessageToChannel(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
        }

        [TearDown]
        public void ResetMocks()
        {
            _tokenRetrieverMock.Reset();
            _slackClientFactoryMock.Reset();
            _slackClientMock.Reset();
        }

        [Test]
        public async Task GetAddToSlackLinkRepeatedly_ReturnsSameLink()
        {
            var firstLink = await GetAddToSlackLink();
            var secondLink = await GetAddToSlackLink();
            
            Assert.AreEqual(firstLink, secondLink);
        }

        [Test]
        public async Task TryToConfirmConnectionWithoutStartingIt_ReturnsNotFound()
        {
            var response = await ConfirmConnection(Guid.NewGuid(), "whatever");
            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task ConfirmConnectionRepeatedly_RetrieveAccessTokenRepeatedly()
        {
            var firstAuthenticationCode = Fixture.Create<string>();
            var secondAuthenticationCode = Fixture.Create<string>();
            var addToSlackLink = await GetAddToSlackLink();
            var connectionId = GetConnectionIdFromAddToSlackLink(addToSlackLink);

            var firstConfirmationResponse = await ConfirmConnection(connectionId, firstAuthenticationCode);
            var secondConfirmationResponse = await ConfirmConnection(connectionId, secondAuthenticationCode);
            
            Assert.AreEqual(HttpStatusCode.OK, firstConfirmationResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, secondConfirmationResponse.StatusCode);
            _tokenRetrieverMock.Verify(retriever => retriever.RetrieveToken(firstAuthenticationCode), Times.Once);
            _tokenRetrieverMock.Verify(retriever => retriever.RetrieveToken(secondAuthenticationCode), Times.Once);
        }

        [Test]
        public async Task ConfirmConnection_ConnectionInfoIsReturnedInOrganizationInfo()
        {
            const string workspaceName = "test_workspace";
            _slackClientMock
                .Setup(client => client.GetInfo())
                .Returns(Task.FromResult(new OrganizationSlackInfo(workspaceName)));
            var authenticationCode = Fixture.Create<string>();
            var addToSlackLink = await GetAddToSlackLink();
            var connectionId = GetConnectionIdFromAddToSlackLink(addToSlackLink);
            await ConfirmConnection(connectionId, authenticationCode);

            var organizationInfo = await HttpClient.GetFromJsonAsync<OrganizationInfo>("organizations/current");
            
            Assert.AreEqual(workspaceName, organizationInfo.SlackInfo.WorkspaceName);
        }
        
        [Test]
        public async Task AddMemberWithSendSlackMessageTriggerAndSetConnection_UsesClientFactoryAndClient()
        {
            var addToSlackLink = await GetAddToSlackLink();
            var connectionId = GetConnectionIdFromAddToSlackLink(addToSlackLink);
            await ConfirmConnection(connectionId, Fixture.Create<string>());
            
            var groupId = await CreateGroupAndGetId();
            
            var sendSlackMessageTrigger = Fixture.Create<SendSlackMessageTrigger>();
            var addTriggerResponse = await AddTrigger(groupId, sendSlackMessageTrigger);
            Assert.AreEqual(HttpStatusCode.OK, addTriggerResponse.StatusCode);

            await AddMember(groupId, Fixture.Create<string>());

            _slackClientFactoryMock.Verify(
                factory => factory.Create(
                    It.IsAny<int>(),
                    It.Is<Option<SlackConnection>>(
                        connection => connection.Exists(
                            c => c.Id == connectionId && c.AccessToken == SlackAccessToken))));
            _slackClientMock.Verify(
                client => client.SendMessageToChannel(
                    sendSlackMessageTrigger.ChannelId, 
                    sendSlackMessageTrigger.MessageTextTemplate));
        }
            
        private async Task<Uri> GetAddToSlackLink()
        {
            var response = await HttpClient.GetAsync("addtoslacklink");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return new Uri(await response.Content.ReadAsStringAsync());
        }

        private static Guid GetConnectionIdFromAddToSlackLink(Uri uri)
        {
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);
            return Guid.Parse(queryParameters["state"]);
        }

        private async Task<HttpResponseMessage> ConfirmConnection(Guid connectionId, string authenticationCode)
        {
            return await HttpClient.PostAsJsonAsync(
                "slackconnection",
                new SlackConnectionConfirmation
                {
                    AuthenticationCode = authenticationCode,
                    ConnectionId = connectionId
                });
        }

        private Mock<ISlackAccessTokenRetriever> _tokenRetrieverMock = new Mock<ISlackAccessTokenRetriever>();
        private Mock<ISlackClientFactory> _slackClientFactoryMock = new Mock<ISlackClientFactory>();
        private Mock<ISlackClient> _slackClientMock = new Mock<ISlackClient>();
        private const string SlackAccessToken = "access_token";
    }
}