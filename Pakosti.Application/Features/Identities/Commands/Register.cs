using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Identities.Commands;

public class Register 
{
    public sealed record Command(
        string Email, DateTime BirthDate, string Password,
        string PasswordConfirm, string FirstName, string LastName) : IRequest<Authenticate.Response>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");
        }
    }
    
    public sealed class Handler : IRequestHandler<Command, Authenticate.Response>
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
        
        public async Task<Authenticate.Response> Handle(Command request, CancellationToken cancellationToken)
        {
            // Todo: check validation

            var user = new AppUser
            {
                Firstname = request.FirstName,
                Lastname = request.LastName,
                Email = request.Email,
                UserName = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new BadRequestException("result is not succeeded");

            var findUser = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (findUser is null) throw new NotFoundException(nameof(user), request.Email);

            await _userManager.AddToRoleAsync(findUser, RoleConstants.Consumer);

            var handler = new Authenticate.Handler(_userManager, _tokenService, _configuration, _repository);
            var command = new Authenticate.Command(
                request.Email, request.Password);
            return await handler.Handle(command, cancellationToken);
        }
    }
}