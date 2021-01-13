using System.Reflection;
using AutoMapper;
using DutyCycle.API.Mapping;
using DutyCycle.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddControllers();

            services.AddAutoMapper(configuration =>
            {
                configuration.AddProfile(typeof(ModelsMappingProfile));
            });

            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRepository, GroupRepository>();

            services.AddDbContext<DutyCycleDbContext>(builder =>
                builder.UseNpgsql(
                    Configuration["ConnectionString"], 
                    optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(Startup).Assembly.FullName)));
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