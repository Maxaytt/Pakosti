using Microsoft.AspNetCore.Identity;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Interfaces;

public interface IIdentityRepository
{
    IEnumerable<IdentityUserRole<Guid>> GetUserRolesById(Guid id, CancellationToken cancellationToken);
    IEnumerable<IdentityRole<Guid>> GetRoles(CancellationToken cancellationToken);
    Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}