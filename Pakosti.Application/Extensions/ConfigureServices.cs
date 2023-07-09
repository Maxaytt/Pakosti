using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pakosti.Application.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        return services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}