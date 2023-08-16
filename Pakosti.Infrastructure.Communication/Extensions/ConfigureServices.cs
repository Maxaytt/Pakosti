using Microsoft.Extensions.DependencyInjection;
using Pakosti.Application.Interfaces;
using Pakosti.Infrastructure.Communication.Services;

namespace Pakosti.Infrastructure.Communication.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureCommunicationServices(
        this IServiceCollection services)
    {
        services.AddHttpClient("CurrencyApiClient");
        return services.AddScoped<ICoefficientService, CoefficientService>();
    }
}