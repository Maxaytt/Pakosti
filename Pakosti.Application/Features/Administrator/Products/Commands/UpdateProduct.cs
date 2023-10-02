using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Administrator.Products.Commands;

public static class UpdateProduct
{
    public sealed record Dto(Guid Id, Guid? CategoryId, string? Name, string? Description);
    public sealed record Command(Guid Id, Guid? CategoryId,
        string? Name, string? Description) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IConfiguration configuration)
        {
            RuleFor(c => c.Name).ProductName(configuration);
            RuleFor(c => c.Description).ProductDescription(configuration);
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
            if (product == null) 
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
            product.EditionDate = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}