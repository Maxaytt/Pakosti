using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Identities.Commands;

public class Register 
{
    public sealed record Command(
        string Email, DateTime BirthDate, string Password,
        string PasswordConfirm, string FirstName, string LastName, string Username) : IRequest<Authenticate.Response>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters");;

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit")
                .Matches("[!@#$%\\^&*()\\[\\]{};':\",.<>\\/\\-=_+]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(64).WithMessage("Firstname must not exceed 64 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(64).WithMessage("Username must not exceed 64 characters");
            
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(5).WithMessage("Username must contain at least 5 characters")
                .MaximumLength(25).WithMessage("Username must not exceed 25 characters");
        }
    }
    
    public sealed class Handler : IRequestHandler<Command, Authenticate.Response>
    {
        private readonly IIdentityRepository _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISender _sender;

        public Handler(UserManager<AppUser> userManager, IIdentityRepository repository, ISender sender)
        {
            _userManager = userManager;
            _repository = repository;
            _sender = sender;
        }
        
        public async Task<Authenticate.Response> Handle(Command request, CancellationToken cancellationToken)
        {
            // Todo: check validation

            var user = new AppUser
            {
                Firstname = request.FirstName,
                Lastname = request.LastName,
                Email = request.Email,
                UserName = request.Username
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new BadRequestException("result is not succeeded");

            var findUser = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (findUser is null) throw new NotFoundException(nameof(user), request.Email);

            await _userManager.AddToRoleAsync(findUser, RoleConstants.Consumer);
            
            var command = new Authenticate.Command(
                request.Email, request.Password);
            return await _sender.Send(command, cancellationToken);
        }
    }
}
