using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pakosti.Application.Behaviours;

namespace Pakosti.Application.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}