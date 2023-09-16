using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Pakosti.Domain.Constants;
using Pakosti.Domain.Entities;

namespace Pakosti.Api.Extensions;

public static class ConfigureAuthorizationExtensions
{
    private static readonly string AdminOnlyPolicy = string.Concat(RoleConstants.Administrator, "Only");
    
    public static IServiceCollection ConfigureAuthorization(
        this IServiceCollection services) => services.AddAuthorization(cfg =>
    {
        cfg.DefaultPolicy =
            new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        
        cfg.AddPolicy(AdminOnlyPolicy, policy =>
            policy.RequireRole(RoleConstants.Administrator));
    });
    
}