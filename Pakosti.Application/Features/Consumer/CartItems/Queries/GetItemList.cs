using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Consumer.CartItems.Queries;

public class GetItemList
{
    public sealed record Query(Guid UserId) : IRequest<Response>;

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
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

            if (cart == null)
                throw new InternalServerException(nameof(Cart), request.UserId, "User cart doesn't exist");

            var items = cart.CartItems.Adapt<List<LookupDto>>();
    
            return new Response(items);
        }
    }

    public sealed record Response(IList<LookupDto> Items);
    public sealed record LookupDto(Guid Id, ProductDto Product, int Amount, decimal TotalCost);
    public sealed record ProductDto(string Name, decimal Cost, string CurrencyName);
}