using System;
using System.Collections.Generic;
using System.Text;
using Calendara.Application.Services;
using Calendara.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Calendara.Application.Validators;
using Calendara.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Calendara.Application
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Changed from Singleton to Scoped for database management and consistency 
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddValidatorsFromAssemblyContaining<EventValidator>(ServiceLifetime.Scoped);
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["Database:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string not found.");
            }

            services.AddDbContext<DatabaseConnection>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped<IDatabaseConnection>(provider => provider.GetRequiredService<DatabaseConnection>());
            services.AddScoped<DbInitializer>();
            return services;
        }
    }
}
