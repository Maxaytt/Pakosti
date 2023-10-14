using MediatR;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Products.Commands;

public static class DeleteProduct
{
    public sealed record Command(Guid Id) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FindAsync(new object[] {request.Id}, cancellationToken);
        
            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}