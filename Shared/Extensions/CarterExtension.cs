using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions
{
    public static class CarterExtension
    {
        public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddCarter(configurator: config =>
            {
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        var modules = assembly.GetTypes()
                        .Where(t => t.IsAssignableTo(typeof(ICarterModule)))
                        .ToArray();

                        config.WithModules(modules);
                    }
                    catch (ReflectionTypeLoadException ex)
                    {

                    }
                    catch(Exception ex)
                    {

                    }
                }
            });

            return services;
        }
    }
}