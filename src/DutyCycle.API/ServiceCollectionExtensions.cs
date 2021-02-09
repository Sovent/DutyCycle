using System.Data.Common;
using DutyCycle.API.Filters;
using DutyCycle.Infrastructure.EntityFramework;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DutyCycle.API
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWorker(this IServiceCollection services)
        {
            services.AddHangfireServer();
        }

        public static IServiceCollection UseDutyCycleDbContext(
            this IServiceCollection serviceCollection,
            string connectionString)
        {
            serviceCollection.AddScoped(serviceProvider =>
            {
                var dbConnection = new NpgsqlConnection(connectionString);
                dbConnection.Open();
                return dbConnection;
            });

            serviceCollection.AddScoped<DbConnection>(serviceProvider =>
                serviceProvider.GetService<NpgsqlConnection>());

            serviceCollection.AddScoped(serviceProvider =>
            {
                var dbConnection = serviceProvider.GetService<DbConnection>();

                return dbConnection.BeginTransaction();
            });

            serviceCollection.AddScoped<DbContextOptions>(serviceProvider =>
            {
                var dbConnection = serviceProvider.GetService<DbConnection>();
                return new DbContextOptionsBuilder<DutyCycleDbContext>()
                    .UseNpgsql(
                        dbConnection,
                        optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(Startup).Assembly.FullName))
                    .Options;
            });

            serviceCollection.AddScoped(serviceProvider =>
            {
                var transaction = serviceProvider.GetService<DbTransaction>();
                var options = serviceProvider.GetService<DbContextOptions>();
                var context = new DutyCycleDbContext(options);
                context.Database.UseTransaction(transaction);
                return context;
            });

            return serviceCollection;
        }
        
        public static IServiceCollection UseOneTransactionPerRequest(this IServiceCollection serviceCollection)
        {
            //Manage the transaction at level of HTTP request/response
            //This is done for every request/response
            serviceCollection.AddScoped(typeof(TransactionFilter), typeof(TransactionFilter));

            serviceCollection
                .AddControllers(options =>
                {
                    options.Filters.AddService<TransactionFilter>(1);
                });

            return serviceCollection;
        }
    }
}