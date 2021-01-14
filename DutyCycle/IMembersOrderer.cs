using System.Collections.Generic;
using LanguageExt.Common;

namespace DutyCycle
{
    public interface IMembersOrderer
    {
        Result<IReadOnlyCollection<GroupMember>> Order(int groupId, IReadOnlyCollection<GroupMember> members);
    }
}