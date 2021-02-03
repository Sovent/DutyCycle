using System;
using DutyCycle.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DutyCycle.IntegrationTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public ApiWebApplicationFactory(Action<IServiceCollection> configureServices)
        {
            _configureServices = configureServices;
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(_configureServices);
        }
        
        private readonly Action<IServiceCollection> _configureServices;
    }
}