using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Calendara.Api.Mapping;
using Calendara.Application;
using Calendara.Application.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Calendara.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Using CORS to integrate Web layer with API
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", builder =>
                {
                    builder.WithOrigins("https://localhost:5001")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    //Fixes bug when sending coordinates in JSON body
                    options.JsonSerializerOptions.NumberHandling = 
                        System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                });
            services.AddApplication();
            services.AddDatabase(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("FrontendPolicy");

            app.UseAuthorization();

            //Mapping registration
            app.UseMiddleware<ValidationMappingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Database initialization during start up
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                initializer.InitializeAsync().Wait();
            }
        }
    }
}
