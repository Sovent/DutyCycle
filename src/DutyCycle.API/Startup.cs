using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using DutyCycle.API.Authentication;
using DutyCycle.API.Filters;
using DutyCycle.API.Mapping;
using DutyCycle.Infrastructure;
using DutyCycle.Infrastructure.EntityFramework;
using DutyCycle.Infrastructure.Json;
using DutyCycle.Infrastructure.Slack;
using DutyCycle.Organizations;
using DutyCycle.Triggers;
using DutyCycle.Users;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionString"];
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<DomainExceptionFilter>();
                    options.Filters.Add<ModelValidationFilter>();
                })
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
                .AddJsonOptions(options =>
                {
                    var serializerOptions = options.JsonSerializerOptions;
                    serializerOptions.Converters.Add(new JsonStringEnumConverter());
                    serializerOptions.Converters.Add(
                        new TypeDiscriminatorJsonConverter<Models.RotationChangedTrigger>());
                });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = "auth_cookie";
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContext =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => 
                    builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
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
            services.AddSingleton<IGroupSettingsValidator, GroupSettingsValidator>();
            services.AddSingleton<IRotationScheduler, RotationScheduler>();
            services.AddSingleton(_ => new SlackTaskClient(Configuration["SlackOAuthToken"]));

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationsService, OrganizationsService>();

            services.AddDbContext<DutyCycleDbContext>(builder =>
            {
                builder.UseNpgsql(
                    connectionString,
                    optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(Startup).Assembly.FullName));
            });

            services
                .AddIdentityCore<Users.User>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DutyCycleDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPermissionsService, UserPermissionsService>();

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

            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}