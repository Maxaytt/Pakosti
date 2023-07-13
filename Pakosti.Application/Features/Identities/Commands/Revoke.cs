using MediatR;
using Microsoft.AspNetCore.Identity;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Identities.Commands;

public class Revoke
{
    public sealed record Command(string Username) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) throw new BadRequestException("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }
    }
}