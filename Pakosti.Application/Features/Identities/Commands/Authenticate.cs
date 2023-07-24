using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Extensions;
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
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters");;

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit")
                .Matches("[!@#$%^&*]").WithMessage("Password must contain at least one special character (!@#$%^&*)");;
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

            var user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken)
                ?? throw new UnauthorizedAccessException();

            var roleIds = _repository
                .GetUserRolesById(user.Id, cancellationToken)
                .Select(r => r.RoleId)
                .ToList();

            var roles = _repository.GetRoles(cancellationToken)
                .Where(x => roleIds.Contains(x.Id))
                .ToList();

            var accessToken = _tokenService.CreateToken(user, roles);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow
                .AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

            await _repository.SaveChangesAsync(cancellationToken);

            return new Response(user.UserName!, user.Email!, accessToken, user.RefreshToken);
        }
    }

    public sealed record Response(string Username, string Email, string Token, string RefreshToken);
}