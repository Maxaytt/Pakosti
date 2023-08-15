using System.Net.Mime;
using System.Text.Json;
using Pakosti.Application.Common.Exceptions;
using ApplicationException = Pakosti.Domain.Exceptions.ApplicationException;

namespace Pakosti.Api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            Title = GetTitle(exception),
            Status = statusCode,
            Detail = exception.Message
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        BadRequestException => StatusCodes.Status400BadRequest,
        ArgumentException => StatusCodes.Status400BadRequest,
        FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
        NotFoundException => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };

    private static string GetTitle(Exception exception) => exception switch
    {
        ApplicationException a => a.Title,
        _ => "Server error"
    };
}

