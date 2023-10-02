using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Roles.Commands;

public static class GetRoles
{
    public sealed record Query : IRequest<Response>;
    
    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public Handler(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var roleNames = await _roleManager.Roles.Select(r => r.Name)
                .ToListAsync(cancellationToken: cancellationToken);
            return new Response(roleNames!);
        }
    }

    public sealed record Response(IList<string> Roles);
}