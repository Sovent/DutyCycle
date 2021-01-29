using System.Text.Json.Serialization;
using AutoMapper;
using DutyCycle.API.Filters;
using DutyCycle.API.Mapping;
using DutyCycle.Infrastructure;
using DutyCycle.Infrastructure.EntityFramework;
using DutyCycle.Infrastructure.Json;
using DutyCycle.Infrastructure.Slack;
using DutyCycle.Triggers;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlackAPI;
using SlackClient = DutyCycle.Infrastructure.Slack.SlackClient;

namespace DutyCycle.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionString"];

            services
                .AddControllers(options => options.Filters.Add<DomainExceptionFilter>())
                .AddJsonOptions(options =>
                {
                    var serializerOptions = options.JsonSerializerOptions;
                    serializerOptions.Converters.Add(new JsonStringEnumConverter());
                    serializerOptions.Converters.Add(
                        new TypeDiscriminatorJsonConverter<Models.RotationChangedTrigger>());
                });

            services.AddAutoMapper(configuration =>
            {
                configuration.AddProfile(typeof(ModelsMappingProfile));
            });

            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddSingleton<ISlackClient, SlackClient>();
            services.AddSingleton<ISlackMessageTemplater, SlackMessageTemplater>();
            services.AddSingleton<TriggersContext>();
            services.AddSingleton<ICronValidator, CronValidator>();
            services.AddSingleton<IGroupSettingsValidator, GroupSettingsValidator>();
            services.AddSingleton<IRotationScheduler, RotationScheduler>();
            services.AddSingleton(_ => new SlackTaskClient(Configuration["SlackOAuthToken"]));

            services.AddDbContext<DutyCycleDbContext>(builder =>
            {
                builder.UseNpgsql(
                    connectionString,
                    optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(Startup).Assembly.FullName));
            });

            services.AddHangfire(configuration => configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(connectionString));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}