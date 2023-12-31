using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Exceptions;
using Pakosti.Domain.Entities;

// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Identities.Commands;

public static class Revoke
{
    public sealed record Command(Guid Id) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) throw new BadRequestException("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }
    }
}