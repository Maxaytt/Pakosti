using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Exceptions;
using ApplicationException = Pakosti.Domain.Exceptions.ApplicationException;

namespace Pakosti.Application.Middlewares;

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
            Detail = exception.Message,
            Errors = GetErrors(exception)
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        BadRequestException => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    private static string GetTitle(Exception exception) => exception switch
    {
        ApplicationException a => a.Title,
        _ => "Server error"
    };

    private IReadOnlyDictionary<string, string[]> GetErrors(Exception exception) => exception switch
    {
        ValidationException validationException => validationException.ErrorsDictionary,
        _ => new Dictionary<string, string[]>()
    };
}

