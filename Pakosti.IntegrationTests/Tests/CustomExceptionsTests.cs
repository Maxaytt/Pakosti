using System.Net.Http.Json;
using Pakosti.Application.Features.Guest.Identities.Commands;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests;

public class CustomExceptionsTests 
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task FailureResponse_ShouldContain_CustomException(HttpClient client)
    {
        var command = new Register.Command(string.Empty, DateTime.UtcNow, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty);
        
        var response = await client.PostAsJsonAsync("/api/identity/register", command);

        var str = await response.Content.ReadAsStringAsync();
        str.ShouldContain("Title");
        str.ShouldContain("Status");
        str.ShouldContain("Detail");
        str.ShouldContain("Errors");
    }
}