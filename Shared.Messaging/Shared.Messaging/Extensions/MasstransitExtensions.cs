﻿using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions
{
    public static class MasstransitExtensions
    {
        public static IServiceCollection AddMassTransitWithAssemblies(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumers(assemblies);

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["RabbitMq:Host"]!), host =>
                    {
                        host.Username(configuration["RabbitMq:Username"]!);
                        host.Password(configuration["RabbitMq:Password"]!);
                    });

                    configurator.AutoDelete = false;
                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}