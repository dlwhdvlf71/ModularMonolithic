using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Data;
using User.Data.Repository;

namespace User
{
    public static class UserModule
    {
        public static IServiceCollection AddUserModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddDbContext<UserDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL:Dev"));
            });

            return services;
        }
    }
}