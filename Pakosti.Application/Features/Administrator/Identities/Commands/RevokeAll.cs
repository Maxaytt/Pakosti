using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Domain.Entities;

// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Identities.Commands;

public static class RevokeAll
{
    public sealed record Command : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}