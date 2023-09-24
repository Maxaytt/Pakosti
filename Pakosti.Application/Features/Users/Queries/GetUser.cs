using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager) => _userManager = userManager;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

            if (user == null) throw new NotFoundException(nameof(AppUser), request.UserId);

            return user.Adapt<Response>();
        }
        public sealed record Response(UserDto Users);
        public sealed record UserDto(Guid UserId, string Email, string Firstname, string Lastname, string Username, IList<string> Roles);
    }
}