using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Products.Commands;

public static class CreateProduct
{
    public sealed record Command(Guid UserId, Guid? CategoryId, string Name, string Description) 
        : IRequest<Guid>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(5).WithMessage("Name must contain at least 5 characters")
                .MaximumLength(150).WithMessage("Name must not exceed 150 characters");
            
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(20).WithMessage("Description must contain at least 5 characters")
                .MaximumLength(1500).WithMessage("Description must not exceed 150 characters");
        }
    }
    
    public sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.CategoryId != null)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
                if (category == null) throw new NotFoundException(nameof(Category), request.CategoryId);
            }
        
            var product = new Product
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                CreationDate = DateTime.Now,
                EditionDate = null,
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}