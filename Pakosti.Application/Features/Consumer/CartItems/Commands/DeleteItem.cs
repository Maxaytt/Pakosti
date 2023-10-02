using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Consumer.CartItems.Commands;

public static class DeleteItem
{
    public sealed record Dto(Guid ItemId);
    public sealed record Command(Guid ItemId, Guid UserId) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, 
                    cancellationToken);
            if (cart is null) throw new InternalServerException(nameof(Cart), request.UserId, "User cart doesn't exist");

            var item = cart.CartItems.FirstOrDefault(i => i.Id == request.ItemId);
            if (item is null) throw new NotFoundException(nameof(CartItem), request.ItemId);

            cart.CartItems.Remove(item);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}