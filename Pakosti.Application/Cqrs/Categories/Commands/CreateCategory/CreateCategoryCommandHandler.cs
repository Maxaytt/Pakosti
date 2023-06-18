using System.Collections.ObjectModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IPakostiDbContext _context;

    public CreateCategoryCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
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