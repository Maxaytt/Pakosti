using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Identities.Commands;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.IdentityTests;

public class IdentityPositiveTests
{
    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task Authenticate_ValidCredentials(HttpClient client)
    {
        // Arrange
        var command = new { Email = "validuser@example.com", Password = "ValidPassword" };

        // Act
        var response = await client.GetAsync("/api/identity/register");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task RefreshToken_ValidRequest_ReturnsNewToken(HttpClient client)
    {
        // Arrange
        var command = new Register.Command(string.Empty, DateTime.Now, string.Empty,
            string.Empty, string.Empty, string.Empty, string.Empty);

        // Act
        var response = await client.PostAsJsonAsync("/api/identity/refresh-token", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseData = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(responseData!.accessToken);
        Assert.NotNull(responseData.refreshToken);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task Register_ValidCommand_ShouldAuthenticate(HttpClient client)
    {
        // Act
        var command = new Register.Command(
            "test@example.com",
            DateTime.UtcNow,
            "TestPassword!1",
            "TestPassword!1",
            "John",
            "Doe",
            "johndoe"
        );
        var content = await client.PostAsJsonAsync("/api/identity/register", command);

        // Assert
        content.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseData = await content.Content.ReadFromJsonAsync<Authenticate.Response>();
        responseData.ShouldBeNull();
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task Revoke_ValidRequest(HttpClient client)
    {
        // Act
        var response = await client.PostAsync("/api/identity/revoke", new StringContent("userId"));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task RevokeAll_ShouldRevoke_AllUsers(HttpClient client)
    {
        // Act
        var response = await client.PostAsync("/api/identity/revoke-all", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var getUsersResponse = await client.GetAsync("/api/identity/users");
        getUsersResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var users = await getUsersResponse.Content.ReadFromJsonAsync<List<RevokeAll>>();
        users.ShouldNotBeNull();
        users.ShouldBeEmpty();
    }
}