using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Pakosti.IntegrationTests.Services;

internal class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly IFixture _fixture;

    public TestWebApplicationFactory(IFixture fixture) => _fixture = fixture;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var postgresContainer = _fixture.Create<PostgreSqlContainer>();

        builder.ConfigureAppConfiguration(configuration => configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["POSTGRES_CONNECTION_STRING"] = postgresContainer.GetConnectionString(),
            ["JWT_EXPIRE"] = "60",
            ["JWT_SECRET"] = "superSecretKey@451",
            ["JWT_ISSUER"] = "https://localhost:5001",
            ["JWT_AUDIENCE"] = "https://localhost:5001",
            ["JWT_TOKEN_VALIDITY_IN_MINUTES"] = "30"
        }));
    }
}