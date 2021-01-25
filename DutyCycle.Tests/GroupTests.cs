using System;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using Cronos;
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
            _fixture.Register(() => CronExpression.Parse("* * * * *"));
            _triggersContext = new TriggersContext(Mock.Of<ISlackClient>(), Mock.Of<ISlackMessageTemplater>());
        }

        [Test]
        public void GetGroupWithoutMembersInfo_EmptyCurrentDutiesAndNextDuties()
        {
            var group = _fixture.Create<Group>();
            var groupInfo = group.Info;
            
            CollectionAssert.IsEmpty(groupInfo.CurrentDuties);
            CollectionAssert.IsEmpty(groupInfo.NextDuties);
        }

        [Test]
        public void GetGroupInfoWhenMembersCountIsLessThatDutiesCount_AllMembersAreCurrentDuties()
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 2);
            var newGroupMemberInfo = _fixture.Create<NewGroupMemberInfo>();
            group.AddMember(newGroupMemberInfo, _triggersContext);

            var groupInfo = group.Info;
            var singleDuty = groupInfo.CurrentDuties.Single();
            
            Assert.AreEqual(newGroupMemberInfo.Name, singleDuty.Name);
            CollectionAssert.IsEmpty(groupInfo.NextDuties);
        }

        [Test]
        public void GetGroupInfoWhenSomeoneIsNotOnDuty_LeavesHimInNextDuties()
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 1);
            var onDutyMemberInfo = _fixture.Create<NewGroupMemberInfo>();
            var notOnDutyMemberInfo = _fixture.Create<NewGroupMemberInfo>();
            group.AddMember(onDutyMemberInfo, _triggersContext);
            group.AddMember(notOnDutyMemberInfo, _triggersContext);

            var groupInfo = group.Info;
            var singleDuty = groupInfo.CurrentDuties.Single();
            var singleNonDuty = groupInfo.NextDuties.Single();
            
            Assert.AreEqual(onDutyMemberInfo.Name, singleDuty.Name);
            Assert.AreEqual(notOnDutyMemberInfo.Name, singleNonDuty.Name);
        }
        
        [Test, AutoData]
        public void AddMemberToEmptyGroup_NoFollowedGroupMemberId(NewGroupMemberInfo newGroupMemberInfo)
        {
            var group = _fixture.Create<Group>();
            group.AddMember(newGroupMemberInfo, _triggersContext);
            var addedMember = group.Members.Single();

            Assert.AreEqual(newGroupMemberInfo.Name, addedMember.Name);
            Assert.IsNull(addedMember.FollowedGroupMemberId);
        }

        [Test, AutoData]
        public void AddNewMemberToNonEmptyGroup_MemberFollowsPreviousMember(
            NewGroupMemberInfo firstGroupMember,
            NewGroupMemberInfo secondGroupMember)
        {
            var group = _fixture.Create<Group>();
            group.AddMember(firstGroupMember, _triggersContext);
            var firstAddedMemberId = group.Members.Single().Id;

            group.AddMember(secondGroupMember, _triggersContext);
            var secondAddedMember = group.Members.Single(member => member.Id != firstAddedMemberId);

            Assert.AreEqual(secondAddedMember.Name, secondGroupMember.Name);
            Assert.AreEqual(firstAddedMemberId, secondAddedMember.FollowedGroupMemberId);
        }

        [Test, AutoData]
        public void AddMemberWithCallback_ExecutesCallback(NewGroupMemberInfo member)
        {
            var group = _fixture.Create<Group>();
            var callbackMock = new Mock<GroupActionTrigger>(Guid.NewGuid(), GroupAction.AddMember);
            group.AddActionTrigger(callbackMock.Object);

            group.AddMember(member, _triggersContext);

            callbackMock.Verify(mock => mock.Run(It.IsAny<GroupInfo>(), _triggersContext), Times.Once);
        }

        [Test, AutoData]
        public void AddMemberWithCallbackForDifferentAction_DoesNotExecuteCallback(NewGroupMemberInfo member)
        {
            var group = _fixture.Create<Group>();
            var callbackMock = new Mock<GroupActionTrigger>(Guid.NewGuid(), GroupAction.RotateDuties);
            group.AddActionTrigger(callbackMock.Object);

            group.AddMember(member, _triggersContext);

            callbackMock.Verify(mock => mock.Run(It.IsAny<GroupInfo>(), _triggersContext), Times.Never);
        }

        [Test, AutoData]
        public void RotateDutiesInEmptyGroup_DoesNothing()
        {
            var group = _fixture.Create<Group>();
            
            group.RotateDuties(_triggersContext);

            CollectionAssert.IsEmpty(group.Members);
        }

        [Test, AutoData]
        public void RotateDutiesInEmptyGroup_DoesNotExecuteCallback()
        {
            var group = _fixture.Create<Group>();
            var callbackMock = new Mock<GroupActionTrigger>(Guid.NewGuid(), GroupAction.RotateDuties);
            group.AddActionTrigger(callbackMock.Object);

            group.RotateDuties(_triggersContext);

            callbackMock.Verify(mock => mock.Run(It.IsAny<GroupInfo>(), It.IsAny<TriggersContext>()), Times.Never);
        }

        [Test, AutoData]
        public void RotateDutiesInGroupOfOnePerson_DoesNothing(NewGroupMemberInfo singleMember)
        {
            var group = _fixture.Create<Group>();
            group.AddMember(singleMember, _triggersContext);

            group.RotateDuties(_triggersContext);
            var newDuty = group.Members.Single();

            Assert.AreEqual(singleMember.Name, newDuty.Name);
        }

        [Test, AutoData]
        public void RotateDutiesInGroupOfOnePerson_DoesNotExecuteCallback(NewGroupMemberInfo singleMember)
        {
            var group = _fixture.Create<Group>();
            group.AddMember(singleMember, _triggersContext);
            var callbackMock = new Mock<GroupActionTrigger>(Guid.NewGuid(), GroupAction.RotateDuties);
            group.AddActionTrigger(callbackMock.Object);

            group.RotateDuties(_triggersContext);

            callbackMock.Verify(mock => mock.Run(It.IsAny<GroupInfo>(), It.IsAny<TriggersContext>()), Times.Never);
        }

        [Test, AutoData]
        public void RotateDutiesWhenThereAreMoreDutiesThenMembers_ReordersMembers(
            NewGroupMemberInfo originalFirst,
            NewGroupMemberInfo originalSecond)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 3);
            group.AddMember(originalFirst, _triggersContext);
            group.AddMember(originalSecond, _triggersContext);

            group.RotateDuties(_triggersContext);
            var members = group.Members.ToArray();
            var newFirst = members[0];
            var newSecond = members[1];

            Assert.AreEqual(originalFirst.Name, newSecond.Name);
            Assert.AreEqual(originalSecond.Name, newFirst.Name);
        }

        [Test, AutoData]
        public void RotateDutiesIntoSameRotation_DoesNothing(
            NewGroupMemberInfo originalFirst,
            NewGroupMemberInfo originalSecond)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 4);
            group.AddMember(originalFirst, _triggersContext);
            group.AddMember(originalSecond, _triggersContext);

            group.RotateDuties(_triggersContext);
            var members = group.Members.ToArray();
            var newFirst = members[0];
            var newSecond = members[1];

            Assert.AreEqual(originalFirst.Name, newFirst.Name);
            Assert.AreEqual(originalSecond.Name, newSecond.Name);
        }

        [Test, AutoData]
        public void RotateDutiesIntoSameRotation_DoesNotExecuteCallback(
            NewGroupMemberInfo originalFirst,
            NewGroupMemberInfo originalSecond)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 4);
            group.AddMember(originalFirst, _triggersContext);
            group.AddMember(originalSecond, _triggersContext);
            var callbackMock = new Mock<GroupActionTrigger>(Guid.NewGuid(), GroupAction.RotateDuties);
            group.AddActionTrigger(callbackMock.Object);

            group.RotateDuties(_triggersContext);

            callbackMock.Verify(mock => mock.Run(It.IsAny<GroupInfo>(), It.IsAny<TriggersContext>()), Times.Never);
        }

        [Test, AutoData]
        public void RotateDutiesWithoutEnoughMembersToPassDutyCompletely_LeavesDutyToSomeOfThePreviousDuties(
            NewGroupMemberInfo memberToRetrieveDutySecondTime,
            NewGroupMemberInfo memberToDisposeFromDuty,
            NewGroupMemberInfo memberToRetrieveDutyFirstTime)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 2);
            group.AddMember(memberToRetrieveDutySecondTime, _triggersContext);
            group.AddMember(memberToDisposeFromDuty, _triggersContext);
            group.AddMember(memberToRetrieveDutyFirstTime, _triggersContext);

            group.RotateDuties(_triggersContext);
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
            NewGroupMemberInfo memberToPassDuty,
            NewGroupMemberInfo memberToRetrieveDuty)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 1);
            group.AddMember(memberToPassDuty, _triggersContext);
            group.AddMember(memberToRetrieveDuty, _triggersContext);

            group.RotateDuties(_triggersContext);
            var newDuty = group.Members.First();

            Assert.AreEqual(memberToRetrieveDuty.Name, newDuty.Name);
        }

        [Test, AutoData]
        public void RotateDutiesWithActualChanges_ExecutesCallback(
            NewGroupMemberInfo memberToPassDuty,
            NewGroupMemberInfo memberToRetrieveDuty)
        {
            var group = new Group(_fixture.Create<string>(), _fixture.Create<CronExpression>(), dutiesCount: 1);
            group.AddMember(memberToPassDuty, _triggersContext);
            group.AddMember(memberToRetrieveDuty, _triggersContext);
            var callbackMock = new Mock<GroupActionTrigger>(Guid.NewGuid(), GroupAction.RotateDuties);
            group.AddActionTrigger(callbackMock.Object);

            group.RotateDuties(_triggersContext);

            callbackMock.Verify(mock => mock.Run(It.IsAny<GroupInfo>(), _triggersContext), Times.Once);
        }
        
        private Fixture _fixture;
        private TriggersContext _triggersContext;
    }
}