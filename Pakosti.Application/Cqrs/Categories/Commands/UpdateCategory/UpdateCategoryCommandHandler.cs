using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Cqrs.Products.Commands.UpdateProduct;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IPakostiDbContext _context;

    public UpdateCategoryCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category == null) throw new NotFoundException(nameof(Category), request.Id);

        if (request.ParentCategoryId != null)
        {
            var parentCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.ParentCategoryId, cancellationToken);
            
            if (parentCategory == null) throw new NotFoundException(nameof(Category), request.ParentCategoryId);
        }
        
        category.ParentCategoryId = request.ParentCategoryId;

        if (request.Name != null)
        {
            category.Name = request.Name;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}