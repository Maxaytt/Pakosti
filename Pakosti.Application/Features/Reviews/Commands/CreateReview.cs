using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Reviews.Commands;

public static class CreateReview
{
    public sealed record Command(Guid UserId, Guid ProductId,
        string Header, string Body) : IRequest<Guid>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            //TODO: add validation
        }
    }
    
    public sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
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
                CreationDate = DateTime.Now,
                EditionDate = null
            };
        
            await _context.Reviews.AddAsync(review, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}