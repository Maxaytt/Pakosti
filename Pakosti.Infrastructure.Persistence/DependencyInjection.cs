using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pakosti.Application.Interfaces;

namespace Pakosti.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];

        services.AddDbContext<PakostiDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPakostiDbContext>(provider => 
            provider.GetService<PakostiDbContext>()!);

        return services;
    }
}