using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Pakosti.Api.Middlewares;
using Pakosti.Api.Services;
using Pakosti.Application.Interfaces;
using Pakosti.Application.Services;

namespace Pakosti.Api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureSwagger(configuration)
            .ConfigureAuthentication(configuration)
            .ConfigureAuthorization()
            .ConfigureIdentity();
        
        services.AddControllers(options => 
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new KebabTransformer()));
        }); 
        services.AddEndpointsApiExplorer();

        services.AddScoped<ITokenService, TokenService>()
            .AddTransient<ExceptionHandlingMiddleware>();
        
        return services;
    }
}