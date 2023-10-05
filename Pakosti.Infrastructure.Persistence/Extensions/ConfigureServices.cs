using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Constants;
using Pakosti.Infrastructure.Persistence.Repositories;
using Pakosti.Infrastructure.Persistence.Services;

namespace Pakosti.Infrastructure.Persistence.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration[SecretKeys.PostgresConnectionString];

        services.AddDbContext<PakostiDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddHealthChecks()
            .AddNpgSql(connectionString!);

        services.AddScoped<IPakostiDbContext>(provider => 
            provider.GetService<PakostiDbContext>()!);

        services
            .AddHostedService<DatabaseInitializer>()
            .AddHostedService<RoleInitializer>()
            .AddHostedService<SuperAdministratorCreator>();
            

        return services.AddScoped<IIdentityRepository, IdentityRepository>();
    }
}