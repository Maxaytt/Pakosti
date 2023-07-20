using Pakosti.Api.Extensions;
using Pakosti.Application.Extensions;
using Pakosti.Application.Middlewares;
using Pakosti.Infrastructure.Persistence.Extensions;
using FluentValidation;
using Pakosti.Application.Features.Identities.Commands;


namespace Pakosti.Api;

public static class Program
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
}

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pakosti v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHealthChecks("/health");
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();
        app.UseEndpoints(builder => builder.MapControllers());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .ConfigurePersistenceServices(_configuration)
            .ConfigureApplicationServices()
            .ConfigureApiServices(_configuration);
        services.AddScoped<IValidator<Authenticate.Response>, ResponseValidator>();
    }
}