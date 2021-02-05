using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using DutyCycle.API.Models;
using DutyCycle.Infrastructure.Json;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using GroupModel = DutyCycle.API.Models.Group;

namespace DutyCycle.IntegrationTests
{
    public abstract class IntegrationTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new ApiWebApplicationFactory(ConfigureServices);
            HttpClient = _factory.CreateClient();
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
            Fixture = new Fixture();
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
            HttpClient.Dispose();
        }
        
        protected virtual void ConfigureServices(IServiceCollection serviceCollection)
        {
            // do nothing   
        }

        protected async Task<int> CreateOrganization(string name = null)
        {
            var newOrganizationInfo = new NewOrganizationInfo()
            {
                Name = name ?? Fixture.Create<string>()
            };
            var createOrganizationResponse = await HttpClient.PostAsJsonAsync("/organizations", newOrganizationInfo);
            
            Assert.AreEqual(HttpStatusCode.OK, createOrganizationResponse.StatusCode);
            var idString = await createOrganizationResponse.Content.ReadAsStringAsync();
            return int.Parse(idString);
        }
        
        protected async Task<int> CreateGroupAndGetId(
            string name = null,
            string cyclingCronExpression = "* * * * *",
            int dutiesCount = 1)
        {
            var groupSettings = new API.Models.GroupSettings()
            {
                Name = name ?? Fixture.Create<string>(),
                CyclingCronExpression = cyclingCronExpression,
                DutiesCount = dutiesCount
            };

            var createGroupResponse = await HttpClient.PostAsJsonAsync("/groups", groupSettings);
            Assert.AreEqual(HttpStatusCode.OK, createGroupResponse.StatusCode);

            var idString = await createGroupResponse.Content.ReadAsStringAsync();
            return int.Parse(idString);
        }

        protected async Task<GroupModel> GetGroup(int id)
        {
            return await HttpClient.GetFromJsonAsync<GroupModel>("/groups/" + id, _jsonSerializerOptions);
        }

        protected async Task<GroupModel[]> GetAllGroups()
        {
            return await HttpClient.GetFromJsonAsync<GroupModel[]>("/groups", _jsonSerializerOptions);
        }

        protected async Task AddMember(int groupId, string name)
        {
            var newMemberInfo = new NewMemberInfo() {Name = name};
            var addMemberResponse =
                await HttpClient.PostAsJsonAsync($"/groups/{groupId}/members", newMemberInfo);
            Assert.AreEqual(HttpStatusCode.OK, addMemberResponse.StatusCode);
        }
        
        private ApiWebApplicationFactory _factory;
        protected HttpClient HttpClient;
        protected IFixture Fixture;
        private JsonSerializerOptions _jsonSerializerOptions;
    }
}