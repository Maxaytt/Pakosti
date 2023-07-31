using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
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
        var registerRequest = new Register.Command("validuser@example.com", DateTime.Today.AddYears(-18), "ValidPassword1!",
            "ValidPassword1!", "firstname", "lastname", "username");
        var authenticateRequest = new Authenticate.Command( "validuser@example.com","ValidPassword1!");

        // Act
        await client.PostAsJsonAsync("/api/identity/register", registerRequest);
        var response = await client.PostAsJsonAsync("/api/identity/login", authenticateRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task RefreshToken_ValidRequest_ReturnsNewToken(HttpClient client)
    {
        // Arrange
        var registerRequest = new Register.Command(
            "testemail@test.com",
            DateTime.Today.AddYears(-18), 
            "passwordD1!", 
            "passwordD1!", 
            "firstname", 
            "lastname", 
            "username");
        var registerResponse = await client.PostAsJsonAsync("/api/identity/register", registerRequest);
        
        var registerData = await registerResponse.Content.ReadFromJsonAsync<Authenticate.Response>();
        var request = new RefreshToken.Command(registerData!.Token, registerData.RefreshToken);

        // Act
        var response = await client.PostAsJsonAsync("/api/identity/refresh-token", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseData = await response.Content.ReadFromJsonAsync<RefreshToken.Response>();
        responseData!.AccessToken.ShouldNotBeNull();
        responseData.AccessToken.ShouldNotBe(registerData.Token);
        responseData.RefreshToken.ShouldNotBeNull();
        responseData.RefreshToken.ShouldNotBe(registerData.RefreshToken);
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task Register_ValidCommand_ShouldAuthenticate(HttpClient client)
    {
        // Act
        var request = new Register.Command(
            "test@example.com",
            DateTime.Today.AddYears(-18),
            "TestPassword!1",
            "TestPassword!1",
            "John",
            "Doe",
            "johndoe");
        var response = await client.PostAsJsonAsync("/api/identity/register", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseData = await response.Content.ReadFromJsonAsync<Authenticate.Response>();
        responseData!.Token.ShouldNotBeNull();
        responseData.RefreshToken.ShouldNotBeNull();
    }

    [Theory(Timeout = 5000)]
    [TestSetup]
    public async Task RevokeAll_ShouldRevoke_AllUsers(HttpClient client)
    {
        // Arrange
        var request = new Register.Command(
            "test@example.com",
            DateTime.Today.AddYears(-18),
            "TestPassword!1",
            "TestPassword!1",
            "John",
            "Doe",
            "johndoe");

        var registerResponse = await client.PostAsJsonAsync("/api/identity/register", request);
        var registerResponseData = registerResponse.Content.ReadFromJsonAsync<Authenticate.Response>();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", registerResponseData.Result!.Token);
        
        // Act
        var response = await client.PostAsync($"/api/identity/revoke/{registerResponseData.Result!.Id}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var getUsersResponse = await client.GetAsync("/api/identity/users");
        getUsersResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}