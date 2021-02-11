using System.Threading.Tasks;
using DutyCycle.Users.Domain;
using LanguageExt;

namespace DutyCycle.Users.Application
{
    public interface IUserService
    {
        Task<int> SignUpUser(UserCredentials credentials, int organizationId);

        Task<Option<User>> FindByCredentials(UserCredentials credentials);

        Task<Option<User>> FindById(int userId);
    }
}