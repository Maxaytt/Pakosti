using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Users.Commands;

public class CreateUser
{
    public sealed record Command(Guid UserId, string Email, DateTime BirthDate, string Password,
        string PasswordConfirm, string FirstName, string LastName, string Username) : IRequest<Response>;

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

    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                Firstname = request.FirstName,
                Lastname = request.LastName,
                Email = request.Email,
                UserName = request.Username
            };
            await _userManager.CreateAsync(user);
            return await Task.FromResult(new Response(user.Id));
        }
    }
    public sealed record Response(Guid UserId);
}