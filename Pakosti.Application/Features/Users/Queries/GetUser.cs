using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Users.Queries;

public class GetUser
{
    public sealed record Query(Guid UserId) : IRequest<Handler.Response>;

    public sealed class Handler : IRequestHandler<Query, Handler.Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

            if (user == null) throw new NotFoundException(nameof(AppUser), request.UserId);

            return user.Adapt<Response>();
        }

        public sealed record Response(string Firstname, string Lastname, string? RefreshToken, DateTimeOffset RefreshTokenExpiryTime);
    }
}