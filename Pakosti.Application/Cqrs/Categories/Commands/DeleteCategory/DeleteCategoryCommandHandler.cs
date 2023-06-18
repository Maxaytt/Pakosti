using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IPakostiDbContext _context;

    public DeleteCategoryCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category == null) throw new NotFoundException(nameof(Category), request.Id);

        _context.Categories.Remove(category);
        await _context.SetNullCategoryChildes(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}