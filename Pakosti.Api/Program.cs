using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pakosti.Api.Extensions;
using Pakosti.Application.Extensions;
using Pakosti.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
            app.UseSwaggerUI();
        }

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();
        app.UseEndpoints(builder =>
        {
            builder.MapControllers();
            builder.MapHealthChecks("/healthz");
            builder.MapHealthChecks("/healthz")
                .RequireHost("*:5001")
                .RequireAuthorization();
            builder.MapHealthChecks("/healthz")
                .RequireAuthorization();
            builder.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("health check")
            });
            builder.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
            });
            builder.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                AllowCachingResponses = true
            });
            
            //check data base
            /*builder.Services.AddHealthChecks()
                .AddSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            //check DbContext
            builder.Services.AddDbContext<SampleDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHealthChecks()
                .AddDbContextCheck<SampleDbContext>();*/
        });
        
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .ConfigurePersistenceServices(_configuration)
            .ConfigureApplicationServices()
            .ConfigureApiServices(_configuration);
        services.AddHealthChecks();
        services.AddHealthChecks()
            .AddTypeActivatedCheck<MyHealthCheck>(
                "Health Check",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "health check" },
                args: new object[] { 1, "Arg" });
    }
}