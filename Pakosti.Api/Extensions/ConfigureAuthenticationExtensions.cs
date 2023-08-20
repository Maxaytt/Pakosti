using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Pakosti.Api.Extensions;

public static class ConfigureAuthenticationExtensions
{
    public static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT_ISSUER"]!,
                    ValidAudience = configuration["JWT_AUDIENCE"]!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(configuration["JWT_SECRET"]!))
                };
            });

        return services;
    }
    
}