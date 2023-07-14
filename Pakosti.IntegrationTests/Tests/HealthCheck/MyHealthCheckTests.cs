using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pakosti.Api;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.HealthCheck
{
    public class MyHealthCheckTests
    {
        [Fact]
        public async Task CheckHealthAsync_ReturnsHealthyResult_WhenIsHealthyIsTrue()
        {
            // Arrange
            var healthCheck = new MyHealthCheck(10, "test")
            {
                IsHealthy = true
            };

            var context = new HealthCheckContext();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await healthCheck.CheckHealthAsync(context, cancellationToken);

            // Assert
            Assert.Equal(HealthStatus.Healthy.ToString(), result.Status.ToString());
            Assert.Equal("A healthy result.", result.Description);
        }

        [Fact]
        public async Task CheckHealthAsync_ReturnsUnhealthyResult_WhenIsHealthyIsFalse()
        {
            // Arrange
            var healthCheck = new MyHealthCheck(10, "test")
            {
                IsHealthy = false
            };

            var context = new HealthCheckContext();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await healthCheck.CheckHealthAsync(context, cancellationToken);

            // Assert
            Assert.Equal(HealthStatus.Unhealthy.ToString(), result.Status.ToString());
            Assert.Equal("An unhealthy result.", result.Description);
        }
    }
}