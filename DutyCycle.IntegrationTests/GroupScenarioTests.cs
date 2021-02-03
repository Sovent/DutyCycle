using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using DutyCycle.API.Models;
using DutyCycle.Infrastructure.Json;
using NUnit.Framework;

using GroupModel = DutyCycle.API.Models.Group;

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
            var name = _fixture.Create<string>();
            var cronExpression = "15 * * * *";
            var dutiesCount = 2;
            
            var createdGroupId = await CreateGroupAndGetId(name, cronExpression, dutiesCount);
            var groups = await _httpClient.GetFromJsonAsync<GroupModel[]>("/groups", _jsonSerializerOptions);
            var createdGroup = groups.Single(group => group.Id == createdGroupId);
            
            Assert.AreEqual(name, createdGroup.Name);
            Assert.AreEqual(cronExpression, createdGroup.CyclingCronExpression);
            Assert.AreEqual(dutiesCount, createdGroup.DutiesCount);
        }

        [Test]
        public async Task EditGroupSettings_ChangesAreRepresentedInGetGroupResponse()
        {
            var groupToEditId = await CreateGroupAndGetId();
            var newGroupSettings = new API.Models.GroupSettings()
            {
                Name = _fixture.Create<string>(),
                CyclingCronExpression = "15 5 5 1 0",
                DutiesCount = 10
            };

            var editGroupResponse = await _httpClient.PutAsJsonAsync("/groups/" + groupToEditId, newGroupSettings);
            Assert.AreEqual(HttpStatusCode.OK, editGroupResponse.StatusCode);

            var editedGroup = await GetGroup(groupToEditId);
            Assert.AreEqual(newGroupSettings.Name, editedGroup.Name);
            Assert.AreEqual(newGroupSettings.CyclingCronExpression, editedGroup.CyclingCronExpression);
            Assert.AreEqual(newGroupSettings.DutiesCount, editedGroup.DutiesCount);
        }

        [Test]
        public async Task AddAndRemoveSendSlackMessageTrigger_ChangesAreRepresentedInGetGroupResponse()
        {
            var groupToChangeTriggersId = await CreateGroupAndGetId();

            var sendSlackMessageTrigger = _fixture.Create<SendSlackMessageTrigger>();

            var addTriggerResponse = await _httpClient.PostAsJsonAsync(
                $"/groups/{groupToChangeTriggersId}/triggers",
                sendSlackMessageTrigger);
            Assert.AreEqual(HttpStatusCode.OK, addTriggerResponse.StatusCode);
            
            var groupWithTrigger = await GetGroup(groupToChangeTriggersId);
            var addedTrigger = (SendSlackMessageTrigger)groupWithTrigger.Triggers.Single();
            Assert.AreEqual(sendSlackMessageTrigger.Id, addedTrigger.Id);
            Assert.AreEqual(sendSlackMessageTrigger.ChannelId, addedTrigger.ChannelId);
            Assert.AreEqual(sendSlackMessageTrigger.MessageTextTemplate, addedTrigger.MessageTextTemplate);

            var removeTriggerResponse =
                await _httpClient.DeleteAsync(
                    $"/groups/{groupToChangeTriggersId}/triggers/{sendSlackMessageTrigger.Id}");
            Assert.AreEqual(HttpStatusCode.OK, removeTriggerResponse.StatusCode);

            var groupWithRemovedTrigger = await GetGroup(groupToChangeTriggersId);
            CollectionAssert.IsEmpty(groupWithRemovedTrigger.Triggers);
        }

        [Test]
        public async Task AddMemberToGroup_MemberShouldAppearWithinGroupMembers()
        {
            var groupToAddMemberId = await CreateGroupAndGetId();
            var memberName = _fixture.Create<string>();

            await AddMember(groupToAddMemberId, memberName);

            var groupWithAddedMember = await GetGroup(groupToAddMemberId);
            var singleMemberInfo = groupWithAddedMember.CurrentDuties.Single();
            Assert.AreEqual(memberName, singleMemberInfo.Name);
        }

        [Test]
        public async Task ForceRotationInGroup_CurrentAndNextDutiesChanged()
        {
            var firstMemberName = _fixture.Create<string>();
            var secondMemberName = _fixture.Create<string>();
            var groupToForceRotationInId = await CreateGroupAndGetId(dutiesCount: 1);
            await AddMember(groupToForceRotationInId, firstMemberName);
            await AddMember(groupToForceRotationInId, secondMemberName);
            
            var groupBeforeRotation = await GetGroup(groupToForceRotationInId);
            var singleDuty = groupBeforeRotation.CurrentDuties.Single();
            var singleNonDuty = groupBeforeRotation.NextDuties.Single();
            Assert.AreEqual(firstMemberName, singleDuty.Name);
            Assert.AreEqual(secondMemberName, singleNonDuty.Name);

            var forceRotationResponse =
                await _httpClient.PostAsJsonAsync($"/groups/{groupToForceRotationInId}/rotations", new { });
            Assert.AreEqual(HttpStatusCode.OK, forceRotationResponse.StatusCode);

            var groupAfterRotation = await GetGroup(groupToForceRotationInId);
            var singleDutyAfterRotation = groupAfterRotation.CurrentDuties.Single();
            var singleNonDutyAfterRotation = groupAfterRotation.NextDuties.Single();
            Assert.AreEqual(firstMemberName, singleNonDutyAfterRotation.Name);
            Assert.AreEqual(secondMemberName, singleDutyAfterRotation.Name);
        }
        
        private async Task<int> CreateGroupAndGetId(
            string name = null,
            string cyclingCronExpression = "* * * * *",
            int dutiesCount = 1)
        {
            var groupSettings = new API.Models.GroupSettings()
            {
                Name = name ?? _fixture.Create<string>(),
                CyclingCronExpression = cyclingCronExpression,
                DutiesCount = dutiesCount
            };

            var createGroupResponse = await _httpClient.PostAsJsonAsync("/groups", groupSettings);
            Assert.AreEqual(HttpStatusCode.OK, createGroupResponse.StatusCode);

            var idString = await createGroupResponse.Content.ReadAsStringAsync();
            return int.Parse(idString);
        }

        private async Task<GroupModel> GetGroup(int id)
        {
            return await _httpClient.GetFromJsonAsync<GroupModel>("/groups/" + id, _jsonSerializerOptions);
        }

        private async Task AddMember(int groupId, string name)
        {
            var newMemberInfo = new NewMemberInfo() {Name = name};
            var addMemberResponse =
                await _httpClient.PostAsJsonAsync($"/groups/{groupId}/members", newMemberInfo);
            Assert.AreEqual(HttpStatusCode.OK, addMemberResponse.StatusCode);
        }

        private ApiWebApplicationFactory _factory;
        private HttpClient _httpClient;
        private IFixture _fixture;
        private JsonSerializerOptions _jsonSerializerOptions;
    }
}