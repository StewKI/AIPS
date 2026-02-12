using System.Text;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Infrastructure.Persistence.Authentication;
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
            Issuer = configuration["JWT_ISSUER"]!,
            Audience = configuration["JWT_AUDIENCE"]!,
            Key = configuration["JWT_KEY"]!,
            ExpirationMinutes = int.Parse(configuration["JWT_EXPIRATION_MINUTES"] ?? "60")
        };
        
        services.AddSingleton(jwtSettings);
        
        services.AddHttpContextAccessor();
        
        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.RequireUniqueEmail = true;
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
        
        return services;
    }
}