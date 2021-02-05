using System.Threading.Tasks;

namespace DutyCycle.Users
{
    public interface IUserService
    {
        Task<int> SignUpUser(UserCredentials credentials, int organizationId);
    }
}