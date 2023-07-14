using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Extensions;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Identities.Commands;

public class RefreshToken
{
    public sealed record Command(string? AccessToken, string? RefreshToken) : IRequest<ObjectResult>;

    public class Validator : AbstractValidator<Command>
    {
        
    }

    public sealed class Handler : IRequestHandler<Command, ObjectResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public Handler(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<ObjectResult> Handle(Command? request, CancellationToken cancellationToken)
        {
            if (request is null) throw new BadRequestException("Invalid client request");

            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;
            var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

            if (principal is null) throw new BadRequestException("Invalid access token or refresh token");

            var username = principal.Identity!.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid access token or refresh token");
            }

            var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _configuration.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }
    }
}