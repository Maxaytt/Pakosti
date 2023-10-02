using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.SuperAdministrator.Currencies.Queries;

public static class GetCurrency
{
    public sealed record Query(string Name) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);
        
            if (currency is null) throw new NotFoundException(nameof(Currency), request.Name);
        
            return new Response(currency);
        }
    }

    public sealed record Response(Currency Currency);
}