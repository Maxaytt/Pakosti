using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Pakosti.Application.Features.Identities.Commands;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.IdentityTests;

public class IdentityNegativeTests
{
    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task Authenticate_InvalidCredentials_ReturnsBadRequest(HttpClient client)
    {
        // Arrange
        var command = new Authenticate.Command("test@example.com","InvalidPassword");

        // Act
        var response = await client.PostAsJsonAsync("/api/identity/register", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task RefreshToken_InvalidRequest_ReturnsBadRequest(HttpClient client)
    {
        // Arrange
        var command = new RefreshToken.Command("invalidAccessToken",  "invalidRefreshToken");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/identity/refresh-token", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task Register_InvalidCommand_ReturnsBadRequest(HttpClient client)
    {
        // Arrange
        var invalidCommand = new Register.Command(
            "invalid_email",
            DateTime.UtcNow,
            "WeakPwd",
            "MismatchedPwd",
            "John",
            "Doe",
            "johndoe"
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/identity/register", invalidCommand);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task Revoke_InvalidRequest_ReturnsBadRequest(HttpClient client)
    {
        // Act
        var response = await client.PostAsync("/api/identity/revoke", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task RevokeAll_InvalidRevoke_AllUsers(HttpClient client)
    {
        // Arrange
        var invalidToken = "invalid_token";

        // Act
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", invalidToken);
        var response = await client.PostAsync("/api/identity/revoke-all", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}