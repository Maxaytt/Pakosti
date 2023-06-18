using MediatR;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IPakostiDbContext _context;

    public DeleteProductCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .FindAsync(new object[] {request.Id}, cancellationToken);
        
        if (entity == null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}