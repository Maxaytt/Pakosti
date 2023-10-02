using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Exceptions;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.SuperAdministrator.UserRoles.Commands;

public static class RemoveRoleFromUser
{
    public sealed record Dto(string Role);
    public sealed record Command(Guid UserId, string Role) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public Handler(RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if(user is null) throw new NotFoundException(nameof(AppUser), request.UserId);

            var role = await _roleManager.FindByNameAsync(request.Role);
            if (role is null) throw new NotFoundException(nameof(IdentityRole<Guid>), request.Role);
            
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains(request.Role)) throw new BadRequestException(
                $"User (id:{user.Id}) don't assigned to role '{request.Role}'");
            
            await _userManager.RemoveFromRolesAsync(user, userRoles);
        }
    }
}