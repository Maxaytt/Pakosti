using System.Reflection;
using Pakosti.Api.Services.Identity;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Api.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureSwagger()
            .ConfigureAuthentication(configuration)
            .ConfigureAuthorization()
            .ConfigureIdentity();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(PakostiDbContext).Assembly));
        });

        services.AddScoped<ITokenService, TokenService>();
        
        return services;
    }
}