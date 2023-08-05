using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Extensions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Identities.Commands;

public static class Authenticate
{
    public sealed record Command(string Email, string Password) : IRequest<Response>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Email).Email();
            RuleFor(c => c.Password).Password();
        }
    }
    
    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IIdentityRepository _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public Handler(UserManager<AppUser> userManager,
            ITokenService tokenService, IConfiguration configuration, IIdentityRepository repository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new BadRequestException("Invalid request");

            var managedUser = await _userManager.FindByEmailAsync(request.Email)
                              ?? throw new BadRequestException("Bad credentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
                throw new BadRequestException("Bad credentials");

            var roleIds = _repository
                .GetUserRolesById(managedUser.Id, cancellationToken)
                .Select(r => r.RoleId)
                .ToList();

            var roles = _repository.GetRoles(cancellationToken)
                .Where(x => roleIds.Contains(x.Id))
                .ToList();

            var accessToken = _tokenService.CreateToken(managedUser, roles);
            managedUser.RefreshToken = _configuration.GenerateRefreshToken();
            managedUser.RefreshTokenExpiryTime = DateTime.UtcNow
                .AddMinutes(_configuration.GetSection("Jwt:TokenValidityInMinutes").Get<int>());

            await _repository.SaveChangesAsync(cancellationToken);

            return new Response(accessToken, managedUser.RefreshToken, managedUser.Id);
        }
    }

    public sealed record Response(string Token, string RefreshToken, Guid Id);
}
