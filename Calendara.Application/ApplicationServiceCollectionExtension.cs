using System;
using System.Collections.Generic;
using System.Text;
using Calendara.Application.Services;
using Calendara.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Calendara.Application.Validators;
using Calendara.Application.Database;

namespace Calendara.Application
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IEventRepository, EventRepository>();
            services.AddValidatorsFromAssemblyContaining<EventValidator>(ServiceLifetime.Singleton);
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IDatabaseConnection>(_ => new DatabaseConnection(connectionString));
            services.AddSingleton<DbInitializer>();
            return services;
        }
    }
}
