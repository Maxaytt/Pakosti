using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Categories.Commands;

public static class CreateCategory
{
    public sealed record Command(Guid? ParentCategoryId, string Name) : IRequest<Guid>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(5).WithMessage("Name must contain at least 5 characters")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
        }
    }

    public sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }
        
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.ParentCategoryId != null)
            {
                var parentCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.ParentCategoryId, cancellationToken);
            
                if (parentCategory == null) throw new NotFoundException(nameof(Category), request.ParentCategoryId);
            }

            var product = new Category
            {
                Id = Guid.NewGuid(),
                ParentCategoryId = request.ParentCategoryId,
                Name = request.Name
            };

            await _context.Categories.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}