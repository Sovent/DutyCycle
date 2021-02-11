using System.Threading.Tasks;

namespace DutyCycle.Users.Application
{
    public interface IUserPermissionsService
    {
        Task ValidateHasAccessToGroup(int userId, int groupId);
    }
}