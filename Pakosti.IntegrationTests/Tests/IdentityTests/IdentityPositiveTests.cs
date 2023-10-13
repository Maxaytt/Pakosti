using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Pakosti.Application.Features.Guest.Identities.Commands;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.IdentityTests;

public class IdentityPositiveTests
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task Authenticate_ValidRequest_ShouldAuthenticate(HttpClient client)
    {
        var registerRequest = new Register.Command("validuser@example.com", DateTime.Today.AddYears(-18), "ValidPassword1!",
            "ValidPassword1!", "firstname", "lastname", "username");
        var authenticateRequest = new Authenticate.Command( "validuser@example.com","ValidPassword1!");

        await client.PostAsJsonAsync("/api/identity/register", registerRequest);
        var response = await client.PostAsJsonAsync("/api/identity/login", authenticateRequest);

        var authenticateResult = response.Content.ReadFromJsonAsync<Authenticate.Response>().Result;
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        authenticateResult!.RefreshToken.ShouldNotBeNull();
        authenticateResult.Token.ShouldNotBeNull();
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task RefreshToken_ValidRequest_ReturnsNewTokens(HttpClient client)
    {
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

        var response = await client.PostAsJsonAsync("/api/identity/refresh-token", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseData = await response.Content.ReadFromJsonAsync<RefreshToken.Response>();
        responseData!.AccessToken.ShouldNotBeNull();
        responseData.AccessToken.ShouldNotBe(registerData.Token);
        responseData.RefreshToken.ShouldNotBeNull();
        responseData.RefreshToken.ShouldNotBe(registerData.RefreshToken);
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task Register_ValidCommand_ShouldAuthenticate(HttpClient client)
    {
        var request = new Register.Command(
            "test@example.com",
            DateTime.Today.AddYears(-18),
            "TestPassword!1",
            "TestPassword!1",
            "John",
            "Doe",
            "johndoe");
        var response = await client.PostAsJsonAsync("/api/identity/register", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseData = await response.Content.ReadFromJsonAsync<Authenticate.Response>();
        responseData!.Token.ShouldNotBeNull();
        responseData.RefreshToken.ShouldNotBeNull();
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task Revoke_ValidRequest_ReturnsOk(HttpClient client)
    {
        var request = new Register.Command(
            "test@example.com",
            DateTime.Today.AddYears(-18),
            "TestPassword!1",
            "TestPassword!1",
            "John",
            "Doe",
            "johndoe");

        var registerResponse = await client.PostAsJsonAsync("/api/identity/register", request);
        var registerResponseData = await registerResponse.Content.ReadFromJsonAsync<Authenticate.Response>();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", registerResponseData!.Token);
        
        var response = await client.PostAsync($"/api/identity/revoke/{registerResponseData.Id}", null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}