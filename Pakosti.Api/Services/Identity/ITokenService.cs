using Microsoft.AspNetCore.Identity;
using Pakosti.Domain.Entities;

namespace Pakosti.Api.Services.Identity;

public interface ITokenService
{
    string CreateToken(AppUser user, IEnumerable<IdentityRole<Guid>> roles);
}