using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Pakosti.Api.Extensions;

public static class ConfigureAuthenticationExtensions
{
    public static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services)
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
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!,
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!))
                };
            });

        return services;
    }
    
}