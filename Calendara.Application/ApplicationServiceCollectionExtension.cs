using System;
using System.Collections.Generic;
using System.Text;
using Calendara.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Calendara.Application
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IEventService, EventService>();

            return services;
        }
    }
}
