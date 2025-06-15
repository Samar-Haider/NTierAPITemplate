using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using NTierAPITemplate.Application.Interfaces;
using NTierAPITemplate.Application.Services;
using NTierAPITemplate.Infrastructure.Data;
using FluentValidation;

namespace NTierAPITemplate.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<NTierAPITemplateDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("Default")));

            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }


        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register all your FluentValidation validators in the Application assembly:
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssembly(typeof(JwtService).Assembly);

            return services;
        }
    }
}
