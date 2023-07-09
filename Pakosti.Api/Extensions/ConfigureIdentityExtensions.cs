using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Api.Extensions;

public static class ConfigureIdentityExtensions
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<PakostiDbContext>()
            .AddUserStore<UserStore<AppUser, IdentityRole<Guid>, PakostiDbContext, Guid>>()
            .AddRoleStore<RoleStore<IdentityRole<Guid>, PakostiDbContext, Guid>>()
            .AddUserManager<UserManager<AppUser>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole<Guid>>();

        return services;
    }
}