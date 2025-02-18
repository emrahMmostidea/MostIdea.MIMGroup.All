﻿using Microsoft.Extensions.DependencyInjection;
using MostIdea.MIMGroup.HealthChecks;

namespace MostIdea.MIMGroup.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<MIMGroupDbContextHealthCheck>("Database Connection");
            builder.AddCheck<MIMGroupDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
