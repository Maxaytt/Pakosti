using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Pakosti.Api.Services;

namespace Pakosti.Api.Extensions;

public static class ConfigureSwaggerExtensions
{
    private const string IgnoredNamespaceIdentifiersKey = "Swagger:IgnoredNamespaceIdentifiers";
    public static IServiceCollection ConfigureSwagger(
        this IServiceCollection services, IConfiguration configuration) => services.AddSwaggerGen(options =>
    {
        options.OperationFilter<AreaTagOperationFilter>();
        
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pakosti", Version = "v1" });
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header,
            Description = "Please enter a valid Token.",
            BearerFormat = "JWT"
        });
        
        options.TagActionsBy(api => 
        {
            if (api.ActionDescriptor is not ControllerActionDescriptor { RouteValues: var routeValues })
                throw new InvalidOperationException("ActionDescriptor is not a ControllerActionDescriptor.");

            var areaName = routeValues!.GetRequiredRouteValue("area");
            var controllerName = routeValues!.GetRequiredRouteValue("controller");

            return new List<string> { $"{areaName}/{controllerName}" };
        });
        
        options.OrderActionsBy((apiDesc) =>
            $"{apiDesc.ActionDescriptor.RouteValues["area"]}/{apiDesc.ActionDescriptor.RouteValues["controller"]}");
        
        options.CustomSchemaIds(type =>
        {
            // Ignored parts of namespaces, generally CQRS-conventional names,
            // such as 'Queries' and 'Commands'. These are skipped when generating
            // Swagger names for the public DTOs.
            var ignoredIdentifiers = configuration
                .GetSection(IgnoredNamespaceIdentifiersKey)
                .Get<HashSet<string>>()!;

            // Generates unique and user-friendly names for CQRS entities.
            // For example, 'Features.Accounts.Commands.Create.Command' gets turned into 'AccountsCreateCommand'.
            var lastNames = type.FullName!.Split('.', '+')
                .Where(identifier => !ignoredIdentifiers.Contains(identifier))
                .TakeLast(3);

            return string.Join(string.Empty, lastNames);
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>() 
            }
        });
    });
    
    private static string GetRequiredRouteValue(this IDictionary<string, string> routeValues, string key)
    {
        if (!routeValues.TryGetValue(key, out var value))
            throw new InvalidOperationException($"Missing '{key}' route value.");
        return value;
    }
}