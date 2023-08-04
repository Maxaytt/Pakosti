using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Reviews.Commands;

public static class UpdateReview
{
    public sealed record Dto(Guid Id, string? Header, string? Body);
    public sealed record Command(Guid Id, Guid UserId,
        string? Header, string? Body ) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Header)
                .MinimumLength(5).WithMessage("Header must contain at least 5 characters")
                .MaximumLength(100).WithMessage("Header must not exceed 150 characters");
            
            RuleFor(c => c.Body)
                .MinimumLength(25).WithMessage("Body must contain at least 5 characters")
                .MaximumLength(1500).WithMessage("Body must not exceed 150 characters");
        }
    }
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (review == null || review.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Review), request.Id);
            }
        
            review.EditionDate = DateTime.UtcNow;
            if (request.Header != null)
            {
                review.Header = request.Header;
            }
            if (request.Body != null)
            {
                review.Body = request.Body;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}