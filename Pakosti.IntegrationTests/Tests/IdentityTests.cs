using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Pakosti.Application.Features.Identities.Commands;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests
{
    public class IdentityTests
    {
        [Theory(Timeout = 5000), TestSetup]
        public async Task AuthenticateEndpoint_InvalidCredentials_ReturnsBadRequest(HttpClient client)
        {
            // Arrange
            var command = new
            {
                Email = "test@example.com",
                Password = "InvalidPassword"
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsJsonAsync("/api/identity/register", jsonContent);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        
        [Theory(Timeout = 5000), TestSetup]
        public async Task RefreshToken_Endpoint_ReturnsNewTokens(HttpClient client)
        {
            // Arrange
            var user = new AppUser
            {
                UserName = "testuser",
                RefreshToken = "valid_refresh_token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(10)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            }));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken());

            // Act
            var command = new RefreshToken.Command(accessToken, "valid_refresh_token");
            var response = await client.PostAsJsonAsync("/api/identity/refresh-token", command);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var responseData = await response.Content.ReadFromJsonAsync<dynamic>();
            Assert.NotNull(responseData!.accessToken);
            Assert.NotNull(responseData.refreshToken);
        }
        
        [Theory(Timeout = 5000), TestSetup]
        public async Task Register_Endpoint_ReturnsAuthenticateResponse(HttpClient client)
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
            var response = await client.PostAsJsonAsync("/api/identity/register", command);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var responseData = await response.Content.ReadFromJsonAsync<Authenticate.Response>();
            Assert.NotNull(responseData);
        }
        
        [Theory(Timeout = 5000), TestSetup]
        public async Task Revoke_Endpoint_InvalidUsername_ReturnsBadRequest(HttpClient client)
        {
            // Act
            var response = await client.PostAsync("/api/identity/revoke", null);
            
            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        
        [Theory(Timeout = 5000), TestSetup]
        public async Task RevokeAll_Endpoint_RevokesAllUserRefreshTokens(HttpClient client)
        {
            // Act
            var response = await client.PostAsync("/api/identity/revoke-all", null);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
    
}
