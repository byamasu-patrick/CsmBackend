using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Infrastructure.Persistence;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.Application.Contracts.Persistence;
using User.Infrastructure.Repositories;
using User.Application.Contracts.Infrastructure;
using User.Infrastructure.Services;
using User.Infrastracture.Services;

namespace User.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
