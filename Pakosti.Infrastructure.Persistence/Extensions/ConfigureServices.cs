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
        var connectionString = configuration["DbConnection"];

        services.AddDbContext<PakostiDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPakostiDbContext>(provider => 
            provider.GetService<PakostiDbContext>()!);

        services
            .AddHostedService<RoleInitializer>()
            .AddHostedService<DatabaseInitializer>();
            
        services.AddHealthChecks().AddSqlServer(connectionString!);
        
        return services.AddScoped<IIdentityRepository, IdentityRepository>();
        
    }
}