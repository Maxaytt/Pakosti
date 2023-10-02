using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Exceptions;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Administrator.Users.Queries;

public static class GetUser
{
    public sealed record Query(Guid UserId) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if(user is null) throw new NotFoundException(nameof(AppUser), request.UserId);
            
            var roles = await _userManager.GetRolesAsync(user);
            
            return new Response(user.Adapt<UserDto>() with { Roles = roles });
        }
    }
    public sealed record Response(UserDto User);
    public sealed record UserDto(Guid UserId, string Email, string Firstname, 
        string Lastname, string Username, IList<string> Roles);
}
