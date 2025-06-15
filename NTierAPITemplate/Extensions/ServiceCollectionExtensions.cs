using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using NTierAPITemplate.Application.Interfaces;
using NTierAPITemplate.Application.Services;
using NTierAPITemplate.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NTierAPITemplate.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NTierAPITemplate.Common.Auth;
using System.Text;

namespace NTierAPITemplate.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<NTierAPITemplateDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("Default")));

            services.AddIdentity<UserAccount, IdentityRole<Guid>>(opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequiredLength = 8;
                    opts.User.RequireUniqueEmail = true;
                    // ...other Identity options...
                })
                .AddEntityFrameworkStores<NTierAPITemplateDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }


        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register all your FluentValidation validators in the Application assembly:
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssembly(typeof(JwtService).Assembly);

            // register your JwtService which now uses UserManager internally
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }


        public static IServiceCollection AddAuthenticationAndJwt(this IServiceCollection services, IConfiguration config)
        {
            // bind settings
            var jwt = config.GetSection("Jwt").Get<JwtSettings>()!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));

            services
              .AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(opts =>
              {
                  opts.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidIssuer = jwt.Issuer,
                      ValidateAudience = true,
                      ValidAudience = jwt.Audience,
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = key,
                      ValidateLifetime = true,
                      ClockSkew = TimeSpan.FromMinutes(1)
                  };
              });

            return services;
        }

    }
}
