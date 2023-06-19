using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Domain.Entities;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Cqrs.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IPakostiDbContext _context;

    public CreateProductCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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