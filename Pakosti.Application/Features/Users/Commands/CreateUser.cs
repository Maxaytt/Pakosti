using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Users.Commands;

public class CreateUser
{
    public sealed record Command(Guid UserId, string Firstname, string Lastname) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IConfiguration configuration)
        {
            RuleFor(c => c.Firstname).FirstName(configuration)
                .NotNull().WithMessage("Firstname is required");
            RuleFor(c => c.Lastname).LastName(configuration)
                .NotNull().WithMessage("Lastname is required");
        }
    }

    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);
            return new Response(user!.Id);
        }
    }

    public sealed record Response(Guid UserId);
}