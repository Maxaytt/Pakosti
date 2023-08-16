using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Extensions;

public static class JwtBearerExtensions
{
    public static IEnumerable<Claim> CreateClaims(this AppUser user, IEnumerable<IdentityRole<Guid>> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Jti, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Role, string.Join(" ", roles.Select(x => x.Name)))
        };
        return claims;
    }

    public static SigningCredentials CreateSigningCredentials(this IConfiguration configuration)
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }

    public static JwtSecurityToken CreateJwtToken(this IEnumerable<Claim> claims, IConfiguration configuration)
    {
        var expireString = Environment.GetEnvironmentVariable("JWT_EXPIRE");
        if (!int.TryParse(expireString, out var expire)) throw new FormatException("JWT_EXPIRE is not number");

            return new JwtSecurityToken(
                Environment.GetEnvironmentVariable("JWT_ISSUER"), 
                Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                claims,
                expires: DateTime.UtcNow.AddMinutes(expire),
                signingCredentials: configuration.CreateSigningCredentials());
    }

    public static JwtSecurityToken CreateToken(this IConfiguration configuration, IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!));
        var validityString = Environment.GetEnvironmentVariable("JWT_TOKEN_VALIDITY_IN_MINUTES");
        if (!int.TryParse(validityString, out var tokenValidityInMinutes))
            throw new FormatException("JWT_TOKEN_VALIDITY_IN_MINUTES is not number");

        var token = new JwtSecurityToken(
            Environment.GetEnvironmentVariable("JWT_ISSUER"), 
            Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
    
    public static string GenerateRefreshToken(this IConfiguration configuration)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static ClaimsPrincipal? GetPrincipalFromExpiredToken(this IConfiguration configuration, string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}