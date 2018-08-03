using System.IO;
using ApiMockServiceMesh.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiMockServiceMesh
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json");

            Configuration = builder.Build();


            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(CustomExceptionFilter));
            });

            services.AddDataProtection(opts =>
            {
                opts.ApplicationDiscriminator = "api-service-mesh";

            });

            services.AddResponseCompression();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "Mock Service Mesh - API", Version = "v1" });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {


            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //loggerFactory.AddDebug(LogLevel.Information);
            //loggerFactory.AddDebug(LogLevel.Debug);
            loggerFactory.AddDebug(LogLevel.Error);
            loggerFactory.AddDebug(LogLevel.Critical);
            loggerFactory.AddDebug(LogLevel.Trace);

            loggerFactory.AddProvider(new CustomLoggerProvider());


            app.UseMvc();
            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Motorista API - V1");
            });
        }
    }
}
