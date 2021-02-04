using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace DutyCycle.API
{
    public static class Worker
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddHangfireServer();
        }
    }
}