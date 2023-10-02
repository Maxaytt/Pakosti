using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Administrator.Users.Queries;

public static class GetUserList
{
    public sealed record Query : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager) => _userManager = userManager;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .ProjectToType<UserDto>()
                .ToListAsync(cancellationToken);

            return new Response(users);
        }
    }

    public sealed record Response(IList<UserDto> Users);
    public sealed record UserDto(Guid UserId, string Email, string FirstName, string LastName, string Username);
}