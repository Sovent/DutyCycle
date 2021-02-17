using System.Threading.Tasks;

namespace DutyCycle.Groups.Domain.Triggers
{
    public interface ITriggersContextFactory
    {
        Task<TriggersContext> CreateContext(int organizationId);
    }
}