using Microsoft.AspNetCore.Identity;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser appUser, IEnumerable<IdentityRole<Guid>> roles);
}