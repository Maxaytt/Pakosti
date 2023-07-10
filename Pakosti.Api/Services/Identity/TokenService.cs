using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Pakosti.Domain.Entities;
using Pakosti.Application.Extensions;
using Pakosti.Application.Interfaces;

namespace Pakosti.Api.Services.Identity;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(AppUser user, IEnumerable<IdentityRole<Guid>> roles)
    {
        var token = user
            .CreateClaims(roles)
            .CreateJwtToken(_configuration);
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}