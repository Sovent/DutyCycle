using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using DutyCycle.Triggers;
using Moq;
using NUnit.Framework;

namespace DutyCycle.Tests
{
    public class GroupTests
    {
        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _triggersTooling = new TriggersTooling(Mock.Of<ISlackClient>());
        }
        
        [Test, AutoData]
        public void AddMemberToEmptyGroup_NoFollowedGroupMemberId(Group group, GroupMemberInfo groupMemberInfo)
        {
            group.AddMember(groupMemberInfo, _triggersTooling);
            var addedMember = group.Members.Single();
            
            Assert.AreEqual(groupMemberInfo.Name, addedMember.Name);
            Assert.IsNull(addedMember.FollowedGroupMemberId);
        }

        [Test, AutoData]
        public void AddNewMemberToNonEmptyGroup_MemberFollowsPreviousMember(
            Group group,
            GroupMemberInfo firstGroupMember,
            GroupMemberInfo secondGroupMember)
        {
            group.AddMember(firstGroupMember, _triggersTooling);
            var firstAddedMemberId = group.Members.Single().Id;
            
            group.AddMember(secondGroupMember, _triggersTooling);
            var secondAddedMember = group.Members.Single(member => member.Id != firstAddedMemberId);
            
            Assert.AreEqual(secondAddedMember.Name, secondGroupMember.Name);
            Assert.AreEqual(firstAddedMemberId, secondAddedMember.FollowedGroupMemberId);
        }

        [Test, AutoData]
        public void RotateDutiesInEmptyGroup_DoesNothing(Group group)
        {
            group.RotateDuties(_triggersTooling);
            
            CollectionAssert.IsEmpty(group.Members);
        }

        [Test, AutoData]
        public void RotateDutiesInGroupOfOnePerson_DoesNothing(Group group, GroupMemberInfo singleMember)
        {
            group.AddMember(singleMember, _triggersTooling);
            
            group.RotateDuties(_triggersTooling);
            var newDuty = group.CurrentDuties.Single();
            
            Assert.AreEqual(singleMember.Name, newDuty.Name);
        }

        [Test, AutoData]
        public void RotateDutiesWhenThereAreMoreDutiesThenMembers_ReordersMembers(
            GroupMemberInfo originalFirst,
            GroupMemberInfo originalSecond)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<string>(), dutiesCount: 3);
            group.AddMember(originalFirst, _triggersTooling);
            group.AddMember(originalSecond, _triggersTooling);
            
            group.RotateDuties(_triggersTooling);
            var currentDuties = group.CurrentDuties.ToArray();
            var newFirst = currentDuties[0];
            var newSecond = currentDuties[1];
            
            Assert.AreEqual(originalFirst.Name, newSecond.Name);
            Assert.AreEqual(originalSecond.Name, newFirst.Name);
        }

        [Test, AutoData]
        public void RotateDutiesIntoSameRotation_DoesNothing(
            GroupMemberInfo originalFirst,
            GroupMemberInfo originalSecond)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<string>(), dutiesCount: 4);
            group.AddMember(originalFirst, _triggersTooling);
            group.AddMember(originalSecond, _triggersTooling);
            
            group.RotateDuties(_triggersTooling);
            var currentDuties = group.CurrentDuties.ToArray();
            var newFirst = currentDuties[0];
            var newSecond = currentDuties[1];
            
            Assert.AreEqual(originalFirst.Name, newFirst.Name);
            Assert.AreEqual(originalSecond.Name, newSecond.Name);
        }

        [Test, AutoData]
        public void RotateDutiesWithoutEnoughMembersToPassDutyCompletely_LeavesDutyToSomeOfThePreviousDuties(
            GroupMemberInfo memberToRetrieveDutySecondTime,
            GroupMemberInfo memberToDisposeFromDuty,
            GroupMemberInfo memberToRetrieveDutyFirstTime)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<string>(), dutiesCount: 2);
            group.AddMember(memberToRetrieveDutySecondTime, _triggersTooling);
            group.AddMember(memberToDisposeFromDuty, _triggersTooling);
            group.AddMember(memberToRetrieveDutyFirstTime, _triggersTooling);
            
            group.RotateDuties(_triggersTooling);
            var currentMembers = group.Members.ToArray();
            var newFirst = currentMembers[0];
            var newSecond = currentMembers[1];
            var newOffDuty = currentMembers[2];
            
            Assert.AreEqual(memberToDisposeFromDuty.Name, newOffDuty.Name);
            Assert.AreEqual(newFirst.Name, memberToRetrieveDutyFirstTime.Name);
            Assert.AreEqual(newSecond.Name, memberToRetrieveDutySecondTime.Name);
        }

        [Test, AutoData]
        public void RotateDutiesWithEnoughMembersToPassDutyCompletely_LeavesDutyToNewMembers(
            GroupMemberInfo memberToPassDuty,
            GroupMemberInfo memberToRetrieveDuty)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<string>(), dutiesCount: 1);
            group.AddMember(memberToPassDuty, _triggersTooling);
            group.AddMember(memberToRetrieveDuty, _triggersTooling);
            
            group.RotateDuties(_triggersTooling);
            var newDuty = group.CurrentDuties.Single();
            
            Assert.AreEqual(memberToRetrieveDuty.Name, newDuty.Name);
        }

        private Fixture _fixture;
        private TriggersTooling _triggersTooling;
    }
}