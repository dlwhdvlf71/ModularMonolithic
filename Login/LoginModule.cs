using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    public static class LoginModule
    {
        public static IServiceCollection AddLoginModule(this IServiceCollection services)
        {
            return services;
        }
    }
}
