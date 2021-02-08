using System.Threading.Tasks;

namespace DutyCycle.Users
{
    public interface IUserPermissionsService
    {
        Task ValidateHasAccessToGroup(int userId, int groupId);
    }
}