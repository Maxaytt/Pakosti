using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Domain.Entities;

// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Pakosti.Application.Features.Consumer.User.Commands;

public static class UpdateUser
{
    public sealed record Dto(string? Email, DateTime? BirthDate, string? Password,
        string? PasswordConfirm, string? FirstName, string? LastName, string? Username);
    public sealed record Command(Guid UserId, string? Email, DateTime? BirthDate, string? Password,
        string? PasswordConfirm, string? FirstName, string? LastName, string? Username) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IConfiguration configuration)
        {
            RuleFor(c => c.Email).Email(configuration);
            RuleFor(c => c.BirthDate).BirthDate();
            RuleFor(c => c.Password).Password(configuration);
            RuleFor(c => c.PasswordConfirm).PasswordConfirm(c => c.Password);
            RuleFor(c => c.FirstName).FirstName(configuration);
            RuleFor(c => c.LastName).LastName(configuration);
            RuleFor(c => c.Username).Username(configuration);
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager) => _userManager = userManager;

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);
            if (user == null) throw new NotFoundException(nameof(AppUser), request.UserId);
            
            user.Email = request.Email ?? user.Email;
            user.Firstname = request.FirstName ?? user.Firstname;
            user.Lastname = request.LastName ?? user.Lastname;
            user.UserName = request.Username ?? user.UserName;
            
            if (request.Password is not null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var changePasswordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
    
                if (!changePasswordResult.Succeeded)
                    throw new InvalidOperationException("Failed to change password");
            }
            
            await _userManager.UpdateAsync(user);
        }
    }
}