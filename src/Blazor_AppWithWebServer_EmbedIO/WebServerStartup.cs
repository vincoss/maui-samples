using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace Blazor_AppWithWebServer
{
    public class WebServerStartup
    {
        public WebServerStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //var args = new string[0];
          //  var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            // MvcServiceCollectionExtensions
            //  services.AddControllers();

            ConfigDependencies(services);
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddControllers();

        //    ConfigDependencies(services);
        //}

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEndpointRouteBuilder endpoits)
        //{
        //    endpoits.MapControllers();
        //}

        public void ConfigDependencies(IServiceCollection services)
        {
        }
    }
}
