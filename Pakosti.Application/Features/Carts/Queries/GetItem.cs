using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Carts.Queries;

public static class GetItem
{
    public sealed record Query(Guid ItemId, Guid UserId) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Price)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, 
                    cancellationToken);
            if (cart is null) throw new InternalServerException(nameof(Cart), request.UserId, "User cart doesn't exist");
        
            var item = cart.CartItems
                .FirstOrDefault(i => i.Id == request.ItemId);
            if (item is null) throw new NotFoundException(nameof(CartItem), request.ItemId);
            
            return item.Adapt<Response>();
        }
    }
    public sealed record Response(Guid Id, ProductDto Product, int Amount, decimal TotalCost);
    public sealed record ProductDto(string Name, decimal Cost, string CurrencyName);
}