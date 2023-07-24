using System.Net;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Testcontainers.MsSql;
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
        response.ShouldBeEquivalentTo(HttpStatusCode.OK);
        content.ShouldBeEquivalentTo("healthy");
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task HealthCheck_DatabaseUnavailable_ShouldBeHealthy(HttpClient client, MsSqlContainer container)
    {
        // Arrange 
        await container.StopAsync();
        
        // Act
        var response = await client.GetAsync("/health");
        
        // Assert
        response.ShouldBeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBeEquivalentTo("unhealthy");
    }
}