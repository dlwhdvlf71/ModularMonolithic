using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Email
{
    public static class EmailModule
    {
        public static IServiceCollection AddEmailModule(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}