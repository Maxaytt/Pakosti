using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Exceptions;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Users.Commands;

public static class AssignRole
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
            if (!userRoles.Any()) throw new InternalServerException(
                nameof(AppUser), user.Id, 
                $"User {user.Id} is not assigned a role");
                
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, role.Name!);
        }
    }
}