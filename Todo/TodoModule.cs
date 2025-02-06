using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Todo
{
    public static class TodoModule
    {
        public static IServiceCollection AddTodoModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL:Dev"));
            });

            return services;
        }
    }
}