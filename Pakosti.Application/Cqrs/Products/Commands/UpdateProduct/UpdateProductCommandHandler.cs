using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IPakostiDbContext _context;

    public UpdateProductCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        
        if (product == null || product.UserId != request.UserId) 
            throw new NotFoundException(nameof(Product), request.Id);
        if (category == null) throw new NotFoundException(nameof(Category), request.CategoryId);
        
        product.CategoryId = request.CategoryId;
        product.EditionDate = DateTime.Now;
        if (request.Name != null)
        {
            product.Name = request.Name;
        }
        if (request.Description != null)
        {
            product.Description = request.Description;
        }


        await _context.SaveChangesAsync(cancellationToken);
    }
}