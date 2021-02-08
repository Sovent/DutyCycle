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
    [TestFixture]
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

        protected async Task<UserCredentials> CreateOrganizationAndAssertSuccess()
        {
            var newOrganizationInfo = new NewOrganizationInfo()
            {
                Name = Fixture.Create<string>(),
                AdminCredentials = new UserCredentials()
                {
                    Email = Fixture.Create<string>() + "@test.com",
                    Password = "Qwerty.123"
                }
            };
            
            var createOrganizationResponse = await CreateOrganizationAndSignIn(newOrganizationInfo);
            
            Assert.AreEqual(HttpStatusCode.OK, createOrganizationResponse.StatusCode);
            
            return newOrganizationInfo.AdminCredentials;
        }

        /// <remarks>
        /// Response with set-cookie implicitly makes HttpClient attach this cookie to next requests
        /// </remarks>
        protected async Task<HttpResponseMessage> CreateOrganizationAndSignIn(NewOrganizationInfo organizationInfo)
        {
            return await HttpClient.PostAsJsonAsync("/organizations", organizationInfo);
        }

        protected async Task SignIn(UserCredentials credentials)
        {
            var signInResponse = await HttpClient.PostAsJsonAsync("/users/signin", credentials);
            
            Assert.AreEqual(HttpStatusCode.OK, signInResponse.StatusCode);
        }
        
        protected async Task SignOut()
        {
            var signOutResponse = await HttpClient.PostAsJsonAsync("/users/signout", new { });
            
            Assert.AreEqual(HttpStatusCode.OK, signOutResponse.StatusCode);
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