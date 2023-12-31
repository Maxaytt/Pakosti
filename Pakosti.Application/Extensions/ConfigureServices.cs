using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pakosti.Application.Behaviours;
using Pakosti.Application.Features.Administrator.Categories.Commands;
using Pakosti.Application.Features.Administrator.Products.Commands;
using Pakosti.Application.Features.Consumer.Reviews.Commands;
using Pakosti.Application.Features.Guest.Identities.Commands;
using Pakosti.Application.Features.SuperAdministrator.Currencies.Commands;
using Pakosti.Application.Mappings;

namespace Pakosti.Application.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services
            .AddTransient<CreateCurrency.Validator>()
            .AddTransient<CreateCategory.Validator>()
            .AddTransient<UpdateCategory.Validator>()
            .AddTransient<Authenticate.Validator>()
            .AddTransient<Register.Validator>()
            .AddTransient<CreateProduct.Validator>()
            .AddTransient<UpdateProduct.Validator>()
            .AddTransient<CreateReview.Validator>()
            .AddTransient<UpdateReview.Validator>();
        
        MapsterConfig.Setup();
        
        return services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}