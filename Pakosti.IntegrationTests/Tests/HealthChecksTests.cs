using System.Net;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Testcontainers.PostgreSql;
using Xunit;

namespace Pakosti.IntegrationTests.Tests;

public class HealthChecksTests
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task HealthCheck_DatabaseAvailable_ShouldBeHealthy(HttpClient client)
    {
        var response = await client.GetAsync("/health");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        content.ShouldBeEquivalentTo("Healthy");
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task HealthCheck_DatabaseUnavailable_ShouldBeHealthy(HttpClient client, PostgreSqlContainer container)
    {
        // Arrange 
        await container.StopAsync();

        // Act
        var response = await client.GetAsync("/health");
        
        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBeEquivalentTo("Unhealthy");
    }
}