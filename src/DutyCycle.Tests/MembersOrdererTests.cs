using System;
using System.Collections.Generic;
using DutyCycle.Groups.Domain;
using LanguageExt;
using LanguageExt.Common;
using NUnit.Framework;

namespace DutyCycle.Tests
{
    public class MembersOrdererTests
    {
        [SetUp]
        public void SetUp()
        {
            _orderer = new MembersOrderer();
        }
        
        [Test]
        public void OrderUnorderedMembers_Success()
        {
            var leadingGroupMember = new GroupMember(Guid.NewGuid(), "Leading", null);
            var trailingGroupMember = new GroupMember(Guid.NewGuid(), "Trailing", leadingGroupMember.Id);
            var unorderedMembers = new[] {trailingGroupMember, leadingGroupMember};
            var expectedResult = new[] {leadingGroupMember, trailingGroupMember};

            var result = _orderer.Order(1, unorderedMembers);
            
           AssertCollectionEqual(expectedResult, result);
        }

        [Test]
        public void OrderSingleMember_ReturnsSameCollection()
        {
            var groupMember = new GroupMember(Guid.NewGuid(), "Member", null);
            var toOrder = new[] {groupMember};

            var result = _orderer.Order(1, toOrder);
            
            AssertCollectionEqual(toOrder, result);
        }

        [Test]
        public void OrderEmptyCollection_ReturnsEmptyCollection()
        {
            var toOrder = Array.Empty<GroupMember>();

            var result = _orderer.Order(1, toOrder);
            
            AssertCollectionEqual(toOrder, result);
        }

        [Test]
        public void OrderTwoNonFollowingGroupMembers_ReturnsNonValidResult()
        {
            var leadingGroupMember = new GroupMember(Guid.NewGuid(), "Leading", null);
            var trailingGroupMember = new GroupMember(Guid.NewGuid(), "Trailing", null);
            var toOrder = new[] {leadingGroupMember, trailingGroupMember};
            var groupId = 1;
            
            var result = _orderer.Order(groupId, toOrder);

            AssertGroupMembersOrderBrokenException(result, groupId);
        }

        [Test]
        public void OrderGroupMembersWithoutLeading_ReturnsNonValidResult()
        {
            var leadingGroupMember = new GroupMember(Guid.NewGuid(), "Leading", Guid.NewGuid());
            var trailingGroupMember = new GroupMember(leadingGroupMember.FollowedGroupMemberId.Value, "Trailing",
                leadingGroupMember.Id);
            var toOrder = new[] {leadingGroupMember, trailingGroupMember};
            var groupId = 1;
            
            var result = _orderer.Order(groupId, toOrder);

            AssertGroupMembersOrderBrokenException(result, groupId);
        }

        private void AssertCollectionEqual(
            IReadOnlyCollection<GroupMember> expected,
            Result<IReadOnlyCollection<GroupMember>> actual)
        {
            actual.Match(members =>
                {
                    CollectionAssert.AreEqual(expected, members);
                    return Unit.Default;
                },
                exception => throw exception);
        }

        private void AssertGroupMembersOrderBrokenException(Result<IReadOnlyCollection<GroupMember>> result, int groupId)
        {
            result.IfSucc(_ => Assert.Fail());
            result.IfFail(exception =>
            {
                var typedException = exception as GroupMembersOrderIsBrokenException;
                Assert.NotNull(typedException);
                Assert.AreEqual(groupId, typedException.GroupId);
            });
        }
        
        private MembersOrderer _orderer;
    }
}