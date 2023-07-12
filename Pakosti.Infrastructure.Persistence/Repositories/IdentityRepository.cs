using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.Repositories;

public class IdentityRepository : IIdentityRepository
{
    private readonly PakostiDbContext _context;

    public IdentityRepository(PakostiDbContext context)
    {
        _context = context;
    }

    public IEnumerable<IdentityUserRole<Guid>> GetUserRolesById(Guid id, CancellationToken cancellationToken) =>
         _context.UserRoles.Where(userRole => userRole.UserId == id);
    
    public IEnumerable<IdentityRole<Guid>> GetRoles(CancellationToken cancellationToken) =>
        _context.Roles;
    
    public async Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>  
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await _context.SaveChangesAsync(cancellationToken);
}