using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using DutyCycle.API.Models;
using NUnit.Framework;

using GroupModel = DutyCycle.API.Models.Group;

namespace DutyCycle.IntegrationTests
{
    public class GroupScenarioTests : IntegrationTests
    {
        [SetUp]
        public async Task SetupOrganization()
        {
            await CreateOrganizationAndAssertSuccess();
        }
        
        [Test]
        public async Task CreateGroup_ShouldAppearInGroupsList()
        {
            var name = Fixture.Create<string>();
            var cronExpression = "15 * * * *";
            var dutiesCount = 2;
            
            var createdGroupId = await CreateGroupAndGetId(name, cronExpression, dutiesCount);
            var groups = await GetAllGroups();
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
                Name = Fixture.Create<string>(),
                CyclingCronExpression = "15 5 5 1 0",
                DutiesCount = 10
            };

            var editGroupResponse = await HttpClient.PutAsJsonAsync("/groups/" + groupToEditId, newGroupSettings);
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

            var sendSlackMessageTrigger = Fixture.Create<SendSlackMessageTrigger>();

            var addTriggerResponse = await HttpClient.PostAsJsonAsync(
                $"/groups/{groupToChangeTriggersId}/triggers",
                sendSlackMessageTrigger);
            Assert.AreEqual(HttpStatusCode.OK, addTriggerResponse.StatusCode);
            
            var groupWithTrigger = await GetGroup(groupToChangeTriggersId);
            var addedTrigger = (SendSlackMessageTrigger)groupWithTrigger.Triggers.Single();
            Assert.AreEqual(sendSlackMessageTrigger.Id, addedTrigger.Id);
            Assert.AreEqual(sendSlackMessageTrigger.ChannelId, addedTrigger.ChannelId);
            Assert.AreEqual(sendSlackMessageTrigger.MessageTextTemplate, addedTrigger.MessageTextTemplate);

            var removeTriggerResponse =
                await HttpClient.DeleteAsync(
                    $"/groups/{groupToChangeTriggersId}/triggers/{sendSlackMessageTrigger.Id}");
            Assert.AreEqual(HttpStatusCode.OK, removeTriggerResponse.StatusCode);

            var groupWithRemovedTrigger = await GetGroup(groupToChangeTriggersId);
            CollectionAssert.IsEmpty(groupWithRemovedTrigger.Triggers);
        }

        [Test]
        public async Task AddMemberToGroup_MemberShouldAppearWithinGroupMembers()
        {
            var groupToAddMemberId = await CreateGroupAndGetId();
            var memberName = Fixture.Create<string>();

            await AddMember(groupToAddMemberId, memberName);

            var groupWithAddedMember = await GetGroup(groupToAddMemberId);
            var singleMemberInfo = groupWithAddedMember.CurrentDuties.Single();
            Assert.AreEqual(memberName, singleMemberInfo.Name);
        }

        [Test]
        public async Task ForceRotationInGroup_CurrentAndNextDutiesChanged()
        {
            var firstMemberName = Fixture.Create<string>();
            var secondMemberName = Fixture.Create<string>();
            var groupToForceRotationInId = await CreateGroupAndGetId(dutiesCount: 1);
            await AddMember(groupToForceRotationInId, firstMemberName);
            await AddMember(groupToForceRotationInId, secondMemberName);
            
            var groupBeforeRotation = await GetGroup(groupToForceRotationInId);
            var singleDuty = groupBeforeRotation.CurrentDuties.Single();
            var singleNonDuty = groupBeforeRotation.NextDuties.Single();
            Assert.AreEqual(firstMemberName, singleDuty.Name);
            Assert.AreEqual(secondMemberName, singleNonDuty.Name);

            var forceRotationResponse =
                await HttpClient.PostAsJsonAsync($"/groups/{groupToForceRotationInId}/rotations", new { });
            Assert.AreEqual(HttpStatusCode.OK, forceRotationResponse.StatusCode);

            var groupAfterRotation = await GetGroup(groupToForceRotationInId);
            var singleDutyAfterRotation = groupAfterRotation.CurrentDuties.Single();
            var singleNonDutyAfterRotation = groupAfterRotation.NextDuties.Single();
            Assert.AreEqual(firstMemberName, singleNonDutyAfterRotation.Name);
            Assert.AreEqual(secondMemberName, singleDutyAfterRotation.Name);
        }
    }
}