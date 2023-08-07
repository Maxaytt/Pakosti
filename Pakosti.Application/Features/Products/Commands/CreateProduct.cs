using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Products.Commands;

public static class CreateProduct
{
    public sealed record Dto(Guid CategoryId, string Name, string Description);
    public sealed record Command(Guid UserId, Guid? CategoryId, string Name, string Description) 
        : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name).ProductName()
                .NotNull().WithMessage("Name is required");
            RuleFor(c => c.Description).ProductDescription()
                .NotNull().WithMessage("Description is required");;
        }
    }
    
    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.CategoryId != null)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
                if (category == null) throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            var product = request.Adapt<Product>();
            product.Id = Guid.NewGuid();
            product.CreationDate = DateTimeOffset.UtcNow;
            product.EditionDate = null;

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(product.Id);
        }
    }

    public sealed record Response(Guid Id);
}