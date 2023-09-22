using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Users.Queries;

public class GetUserList
{
    public sealed record Query : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .ProjectToType<LookupDto>()
                .ToListAsync(cancellationToken);

            return new Response(users);
        }
    }

    public sealed record Response(IList<LookupDto> Users);

    public sealed record LookupDto(Guid UserId);
}