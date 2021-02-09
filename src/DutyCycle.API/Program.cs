using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DutyCycle.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            
            if (args.Contains("--worker"))
            {
                ConfigureServicesForWorker(hostBuilder);
            }
            
            hostBuilder.Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureServicesForWorker(IHostBuilder builder)
        {
            builder.ConfigureServices(ServiceCollectionExtensions.ConfigureWorker);
        }
    }
}