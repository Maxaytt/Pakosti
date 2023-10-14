using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Exceptions;
using Pakosti.Domain.Entities;

// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Users.Commands;

public static class DeleteUser
{
    public sealed record Command(Guid UserId) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager) => _userManager = userManager;
            
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null) throw new NotFoundException(nameof(AppUser), request.UserId);

            await _userManager.DeleteAsync(user);
        }
    }
}