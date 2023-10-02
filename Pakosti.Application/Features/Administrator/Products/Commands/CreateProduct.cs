using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Products.Commands;

public static class CreateProduct
{
    public sealed record Dto(Guid CategoryId, string Name, string Description, decimal Cost, string Currency);
    public sealed record Command(Guid? CategoryId, string Name, string Description, decimal Cost, string Currency) 
        : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IConfiguration configuration)
        {
            RuleFor(c => c.Name).ProductName(configuration)
                .NotNull().WithMessage("Name is required");
            RuleFor(c => c.Description).ProductDescription(configuration)
                .NotNull().WithMessage("Description is required");
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

            var currency = await _context.Currencies
                    .FirstOrDefaultAsync(c => c.Name == request.Currency, cancellationToken);
            if (currency is null) throw new NotFoundException(nameof(Currency), request.Currency);

            var product = request.Adapt<Product>();
            product.Id = Guid.NewGuid();
            product.CreationDate = DateTimeOffset.UtcNow;
            product.EditionDate = null;
            product.Price = new Price
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                CurrencyName = request.Currency,
                Cost = request.Cost,
                Currency = currency
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(product.Id);
        }
    }

    public sealed record Response(Guid Id);
}