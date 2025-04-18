using System;
using System.Collections.Generic;
using System.Text;
using Calendara.Application.Services;
using Calendara.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Calendara.Application.Validators;

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
    }
}
