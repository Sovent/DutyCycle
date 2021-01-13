using System.Linq;
using AutoFixture.NUnit3;
using NUnit.Framework;

namespace DutyCycle.Tests
{
    public class GroupTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, AutoData]
        public void AddMemberToEmptyGroup_NoFollowedGroupMemberId(Group group, GroupMemberInfo groupMemberInfo)
        {
            group.AddMember(groupMemberInfo);
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
            group.AddMember(firstGroupMember);
            var firstAddedMemberId = group.Members.Single().Id;
            
            group.AddMember(secondGroupMember);
            var secondAddedMember = group.Members.Single(member => member.Id != firstAddedMemberId);
            
            Assert.AreEqual(secondAddedMember.Name, secondGroupMember.Name);
            Assert.AreEqual(firstAddedMemberId, secondAddedMember.FollowedGroupMemberId);
        }
    }
}