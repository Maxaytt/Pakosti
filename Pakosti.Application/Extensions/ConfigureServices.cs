using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pakosti.Application.Behaviours;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services.AddScoped<IIdentityRepository, IIdentityRepository>();
    }
}