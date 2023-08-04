using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
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
        private const int NameMinLength = 5;
        private const int NameMaxLength = 150;
        private const int DescriptionMinLength = 20;
        private const int DescriptionMaxLength = 1500;
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(NameMinLength)
                .WithMessage($"Name must contain at least {NameMinLength} characters")
                .MaximumLength(NameMaxLength)
                .WithMessage($"Name must not exceed {NameMaxLength} characters");
            
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(DescriptionMinLength)
                .WithMessage($"Description must contain at least {DescriptionMinLength} characters")
                .MaximumLength(DescriptionMaxLength)
                .WithMessage($"Description must not exceed {DescriptionMaxLength} characters");
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
            product.CreationDate = DateTime.UtcNow;
            product.EditionDate = null;

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(product.Id);
        }
    }

    public sealed record Response(Guid Id);
}