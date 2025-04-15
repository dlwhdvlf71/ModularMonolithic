using Microsoft.Extensions.DependencyInjection;
using Shared.Mediator;
using System.Reflection;

namespace Shared.Extensions
{
    public static class CustomMediatRExtension
    {
        public static IServiceCollection AddCustomMediatRWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped<ICustomMediator, CustomMediator>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var iface in type.GetInterfaces())
                    {
                        if (!iface.IsGenericType) continue;

                        var def = iface.GetGenericTypeDefinition();
                        if (def == typeof(ICustomRequestHandler<,>) || def == typeof(ICustomNotificationHandler<>))
                            services.AddScoped(iface, type);

                        if (def == typeof(ICustomPipelineBehavior<,>))
                            services.AddScoped(typeof(ICustomPipelineBehavior<,>), type);
                    }
                }
            }

            return services;
        }
    }
}