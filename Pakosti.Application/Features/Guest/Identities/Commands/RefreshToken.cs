using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Extensions;
using Pakosti.Domain.Entities;

// ReSharper disable UnusedType.Global

namespace Pakosti.Application.Features.Guest.Identities.Commands;

public static class RefreshToken
{
    public sealed record Command(string? AccessToken, string? RefreshToken) : IRequest<Response>;
    
    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public Handler(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Response> Handle(Command? request, CancellationToken cancellationToken)
        {
            if (request is null) throw new BadRequestException("Invalid client request");

            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;
            var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

            if (principal is null) throw new BadRequestException("Invalid access token or refresh token");

            var username = principal.Identity!.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTimeOffset.UtcNow)
            {
                throw new BadRequestException("Invalid access token or refresh token");
            }

            var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _configuration.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new Response(new JwtSecurityTokenHandler().WriteToken(newAccessToken), newRefreshToken);
        }
    }
    public sealed record Response(string AccessToken, string? RefreshToken);
}