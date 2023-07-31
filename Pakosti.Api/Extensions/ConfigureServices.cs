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
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddScoped<ITokenService, TokenService>();
        
        return services;
    }
}