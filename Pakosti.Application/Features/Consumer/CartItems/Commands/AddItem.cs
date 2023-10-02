using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Consumer.CartItems.Commands;

public static class AddItem
{
    public sealed record Dto(Guid ProductId, int Amount);
    public sealed record Command(Guid ProductId, int Amount, Guid UserId) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(c => c.Amount).GreaterThan(0)
            .WithMessage("Amount must be at least 1");
    }
    
    //todo: add amount/stock constraint

    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.Price)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId,
                    cancellationToken);
            if (product is null) throw new NotFoundException(nameof(Product), request.ProductId);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, 
                    cancellationToken);
            if (cart is null) throw new InternalServerException(nameof(Cart), request.UserId, "User cart doesn't exist");
        
            var item = new CartItem
            {
                CartId = request.UserId,
                Id = request.ProductId,
                Product = product,
                Cart = cart,
                CostOfOne = product.Price.Cost,
                Amount = request.Amount
            };
        
            await _context.CartItems.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new Response(
                item.Product.Id,
                item.Product.Name,
                item.CostOfOne,
                item.Amount);
        }
    }
    
    public sealed record Response(Guid Id, string Name, decimal Price, int Amount);
}