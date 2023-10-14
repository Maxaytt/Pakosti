using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Pakosti.Application.Features.Consumer.CartItems.Commands;

public static class UpdateItem
{
    public sealed record Dto(Guid ItemId, int Amount);
    public sealed record Command(Guid ItemId, int Amount, Guid UserId) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(c => c.Amount).GreaterThan(0)
            .WithMessage("Amount must be at least 1");
    }

    //todo: add amount/stock constraint
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

            item.Amount = request.Amount;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}