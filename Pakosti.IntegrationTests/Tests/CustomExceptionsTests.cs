using System.Net.Http.Json;
using Pakosti.Application.Features.Identities.Commands;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests;

public class CustomExceptionsTests 
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task FailureResponse_ShouldContain_CustomException(HttpClient client)
    {
        // Arrange
        var command = new Register.Command(string.Empty, DateTime.Now, string.Empty,
            string.Empty, string.Empty, string.Empty, string.Empty);
        
        // Act
        var response = await client.PostAsJsonAsync("/api/identity/register", command);

        // Assert
        var str = await response.Content.ReadAsStringAsync();
        str.ShouldContain("Title");
        str.ShouldContain("Status");
        str.ShouldContain("Detail");
        str.ShouldContain("Errors");
    }
}