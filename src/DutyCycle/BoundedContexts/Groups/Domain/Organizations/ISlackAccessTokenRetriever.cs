using System.Threading.Tasks;

namespace DutyCycle.Groups.Domain.Organizations
{
    public interface ISlackAccessTokenRetriever
    {
        Task<string> RetrieveToken(string authenticationCode);
    }
}