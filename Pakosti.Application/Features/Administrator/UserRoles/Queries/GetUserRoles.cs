using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Exceptions;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Administrator.UserRoles.Queries;

public static class GetRole
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
            if (roles.Count > 1) throw new InternalServerException(nameof(AppUser), user.Id, 
                "User have more then 1 role");

            return new Response(roles.First());
        }
    }

    public sealed record Response(string Role);
}