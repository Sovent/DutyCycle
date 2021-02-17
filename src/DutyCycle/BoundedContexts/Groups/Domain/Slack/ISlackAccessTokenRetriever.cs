using System.Threading.Tasks;

namespace DutyCycle.Groups.Domain.Slack
{
    public interface ISlackAccessTokenRetriever
    {
        Task<string> RetrieveToken(string authenticationCode);
    }
}