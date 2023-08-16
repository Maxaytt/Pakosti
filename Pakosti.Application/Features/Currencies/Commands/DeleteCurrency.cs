using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Currencies.Commands;

public static class DeleteCurrency
{
    public sealed record Command(string Name) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);

            _context.Currencies.Remove(currency!);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}