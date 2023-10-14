using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Pakosti.Api.Services;

// ReSharper disable once ClassNeverInstantiated.Global
public class AreaTagOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor actionDescriptor) return;
        
        if (!actionDescriptor.RouteValues.TryGetValue("area", out var areaName)
            || !actionDescriptor.RouteValues.TryGetValue("controller", out var controllerName)) return;
        var combinedTagName = $"{areaName}/{controllerName}"; 
        operation.Tags = new List<OpenApiTag> { new() { Name = combinedTagName } };
    }
}