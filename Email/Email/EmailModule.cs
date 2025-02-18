using Microsoft.Extensions.DependencyInjection;

namespace Email
{
    public static class EmailModule
    {
        public static IServiceCollection AddEmailModule(this IServiceCollection services)
        {
            return services;
        }
    }
}