using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using DutyCycle.API.Models;
using DutyCycle.Infrastructure.Json;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace DutyCycle.IntegrationTests
{
    [TestFixture]
    public class GroupScenarioTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new ApiWebApplicationFactory();
            _httpClient = _factory.CreateClient();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonSerializerOptions.Converters.Add(
                new TypeDiscriminatorJsonConverter<RotationChangedTrigger>());
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
            _httpClient.Dispose();
        }

        [Test]
        public async Task CreateGroup_ShouldAppearInGroupsList()
        {
            var groupSettings = new API.Models.GroupSettings()
            {
                Name = _fixture.Create<string>(),
                CyclingCronExpression = "* * * * *",
                DutiesCount = 1
            };

            var createGroupResponse = await _httpClient.PostAsJsonAsync("/groups", groupSettings);
            Assert.AreEqual(HttpStatusCode.OK, createGroupResponse.StatusCode);

            var groups = await _httpClient.GetFromJsonAsync<API.Models.Group[]>("/groups", _jsonSerializerOptions);
            Assert.IsTrue(groups.Any(group => group.Name == groupSettings.Name));
        }

        private ApiWebApplicationFactory _factory;
        private HttpClient _httpClient;
        private IFixture _fixture;
        private JsonSerializerOptions _jsonSerializerOptions;
    }
}