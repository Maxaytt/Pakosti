using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Pakosti.Application.Features.SuperAdministrator.Currencies.Queries;

public static class GetCurrencyList
{
    public sealed record Query : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var currencies = await _context.Currencies
                .ProjectToType<LookupDto>()
                .ToListAsync(cancellationToken);

            return new Response(currencies);
        }
    }

    public sealed record Response(IList<LookupDto> Currencies);

    public sealed record LookupDto(string Name, decimal Coefficient);
}