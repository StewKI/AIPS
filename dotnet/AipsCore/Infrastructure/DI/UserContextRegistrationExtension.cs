using System.Text;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Authentication;
using AipsCore.Domain.Models.User.Options;
using AipsCore.Infrastructure.DI.Configuration;
using AipsCore.Infrastructure.Persistence.Authentication;
using AipsCore.Infrastructure.Persistence.Authentication.AuthService;
using AipsCore.Infrastructure.Persistence.Authentication.Jwt;
using AipsCore.Infrastructure.Persistence.Authentication.UserContext;
using AipsCore.Infrastructure.Persistence.Db;
using AipsCore.Infrastructure.Persistence.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AipsCore.Infrastructure.DI;

public static class UserContextRegistrationExtension
{
    public static IServiceCollection AddUserContext(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings
        {
            Issuer = configuration.GetEnvJwtIssuer(),
            Audience = configuration.GetEnvJwtAudience(),
            Key = configuration.GetEnvJwtKey(),
            ExpirationMinutes = configuration.GetEnvJwtExpirationMinutes(),
            RefreshTokenExpirationDays = configuration.GetEnvJwtRefreshExpirationDays()
        };
        
        services.AddSingleton(jwtSettings);
        
        services.AddHttpContextAccessor();
        
        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = UserOptionsDefaults.PasswordRequiredLength;
                options.Password.RequireDigit = UserOptionsDefaults.PasswordRequireDigit;
                options.Password.RequireLowercase = UserOptionsDefaults.PasswordRequireLowercase;
                options.Password.RequireUppercase = UserOptionsDefaults.PasswordRequireUppercase;
                options.Password.RequireNonAlphanumeric = UserOptionsDefaults.PasswordRequireNonAlphanumeric;

                options.User.RequireUniqueEmail = UserOptionsDefaults.UserRequireUniqueEmail;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AipsDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });
        
        services.AddAuthorization();
        
        services.AddTransient<IUserContext, HttpUserContext>();
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        services.AddTransient<IAuthService, EfAuthService>();
        
        return services;
    }
}