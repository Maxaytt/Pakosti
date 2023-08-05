using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Reviews.Commands;

public static class CreateReview
{
    public sealed record Dto(Guid ProductId, string Header, string Body);
    public sealed record Command(Guid UserId, Guid ProductId,
        string Header, string Body) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Header).ReviewHeader()
                .NotNull().WithMessage("Header is required");
            RuleFor(c => c.Body).ReviewBody()
                .NotNull().WithMessage("Body is required");
        }
    }
    
    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, CancellationToken.None);

            if (product == null) throw new NotFoundException(nameof(Product), request.ProductId);
        
            var review = new Review
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ProductId = request.ProductId,
                Header = request.Header,
                Body = request.Body,
                CreationDate = DateTime.UtcNow,
                EditionDate = null
            };
        
            await _context.Reviews.AddAsync(review, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(review.Id);
        }
    }

    public sealed record Response(Guid Id);
}