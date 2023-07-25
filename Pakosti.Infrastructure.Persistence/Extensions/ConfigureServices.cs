using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pakosti.Application.Interfaces;
using Pakosti.Infrastructure.Persistence.Repositories;
using Pakosti.Infrastructure.Persistence.Services;

namespace Pakosti.Infrastructure.Persistence.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["POSTGRES_CONNECTION_STRING"];

        services.AddDbContext<PakostiDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddHealthChecks()
            .AddSqlServer(connectionString!);

        services.AddScoped<IPakostiDbContext>(provider => 
            provider.GetService<PakostiDbContext>()!);

        services
            .AddHostedService<DatabaseInitializer>()
            .AddHostedService<RoleInitializer>();
            

        return services.AddScoped<IIdentityRepository, IdentityRepository>();
    }
}