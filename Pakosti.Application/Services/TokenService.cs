using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Extensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(AppUser appUser, IEnumerable<IdentityRole<Guid>> roles)
    {
        var token = appUser
            .CreateClaims(roles)
            .CreateJwtToken(_configuration);
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}