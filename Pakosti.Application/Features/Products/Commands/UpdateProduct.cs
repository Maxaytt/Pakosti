using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Products.Commands;

public static class UpdateProduct
{
    public sealed record Dto(Guid Id, Guid? CategoryId, string? Name, string? Description);
    public sealed record Command(Guid Id, Guid UserId, Guid? CategoryId,
        string? Name, string? Description) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .MinimumLength(5).WithMessage("Name must contain at least 5 characters")
                .MaximumLength(150).WithMessage("Name must not exceed 150 characters");
            
            RuleFor(c => c.Description)
                .MinimumLength(20).WithMessage("Description must contain at least 5 characters")
                .MaximumLength(1500).WithMessage("Description must not exceed 150 characters");
        }
    }
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

            public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (product == null || product.UserId != request.UserId) 
                throw new NotFoundException(nameof(Product), request.Id);
            
            if (request.CategoryId != null)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
                if (category == null) throw new NotFoundException(nameof(Category), request.CategoryId);
                product.CategoryId = request.CategoryId;
            }
            if (request.Name != null)
            {
                product.Name = request.Name;
            }
            if (request.Description != null)
            {
                product.Description = request.Description;
            }
            product.EditionDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}