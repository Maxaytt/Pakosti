using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Constants;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Identities.Commands;

public static class Register 
{
    public sealed record Command(
        string Email, DateTime BirthDate, string Password,
        string PasswordConfirm, string FirstName, string LastName, string Username) : IRequest<Authenticate.Response>;
    
    public class Validator : AbstractValidator<Command>
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
    
    
    public sealed class Handler : IRequestHandler<Command, Authenticate.Response>
    {
        private readonly IIdentityRepository _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPakostiDbContext _context;
        private readonly ISender _sender;

        public Handler(UserManager<AppUser> userManager, IIdentityRepository repository,
            ISender sender, IPakostiDbContext context)
        {
            _userManager = userManager;
            _repository = repository;
            _sender = sender;
            _context = context;
        }
        //todo: User already exists exception
        public async Task<Authenticate.Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                Firstname = request.FirstName,
                Lastname = request.LastName,
                Email = request.Email,
                UserName = request.Username
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var duplicateUsername = result.Errors.FirstOrDefault(e => e.Code == "DuplicateUserName");
                var duplicateEmail = result.Errors.FirstOrDefault(e => e.Code == "DuplicateEmail");

                throw (duplicateUsername, duplicateEmail) switch
                {
                    (not null, _) => throw new ConflictException(nameof(AppUser), request.Username,
                        $"User with username \"{request.Username}\" already exists."),
                    (_, not null) => throw new ConflictException(nameof(AppUser), request.Email,
                        $"User with email \"{request.Email}\" already exists."),
                    _ => throw new BadRequestException("User creation failed for unknown reasons.")
                };
            }

            var findUser = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (findUser is null) throw new NotFoundException(nameof(user), request.Email);

            await _userManager.AddToRoleAsync(findUser, Roles.Consumer);
            
            await _context.Carts.AddAsync(new Cart { UserId = findUser.Id }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var command = new Authenticate.Command(
                request.Email, request.Password);
            return await _sender.Send(command, cancellationToken);
        }
    }
}
