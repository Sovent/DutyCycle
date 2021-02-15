using System;
using DutyCycle.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutyCycle.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var commandLineApplication = new CommandLineApplication();
            var startAsWorker = commandLineApplication.Option(
                "--worker", 
                "Start as background jobs processor",
                CommandOptionType.NoValue);
            var migrateDatabase = commandLineApplication.Option(
                "--migrate",
                "Update application database to the last version", 
                CommandOptionType.NoValue);
            commandLineApplication.HelpOption("--help");
            commandLineApplication.OnExecute(() => ExecuteProgram(args, startAsWorker, migrateDatabase));

            commandLineApplication.Execute(args);
        }

        private static int ExecuteProgram(
            string[] args,
            CommandOption startAsWorkerOption,
            CommandOption migrateDatabaseOption)
        {
            if (startAsWorkerOption.HasValue() && migrateDatabaseOption.HasValue())
            {
                Console.WriteLine("Database migration is supposed to return exit code, while worker can't exit.");
                Console.WriteLine("Can't use --worker and --migrate simultaneously");
                return 1;
            }
            
            var hostBuilder = CreateHostBuilder(args);
            if (startAsWorkerOption.HasValue())
            {
                ConfigureServicesForWorker(hostBuilder);
            }

            var host = hostBuilder.Build();
            if (migrateDatabaseOption.HasValue())
            {
                Console.WriteLine("Applying database migrations");
                using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                using var context = serviceScope.ServiceProvider.GetService<DutyCycleDbContext>();
                context.Database.Migrate();
                Console.WriteLine("Migrations finished");
                return 0;
            }
            
            host.Run();
            return 0;
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureServicesForWorker(IHostBuilder builder)
        {
            builder.ConfigureServices(Worker.Configure);
        }
    }
}