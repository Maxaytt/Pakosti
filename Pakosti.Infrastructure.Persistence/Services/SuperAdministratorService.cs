using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Validators;
using Pakosti.Domain.Constants;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.Services;

public class SuperAdministratorService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    
    private Lazy<string?> Username => new(() => _configuration[SecretKeys.SuperAdmin.Username]);
    private Lazy<string?> Email => new(() => _configuration[SecretKeys.SuperAdmin.Email]);
    private Lazy<string?> Password => new(() => _configuration[SecretKeys.SuperAdmin.Password]);
    
    public SuperAdministratorService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    
    public async Task CreateSuperAdministrator(CancellationToken cancellationToken)
    {
        ThrowIfInvalidPassword(Password.Value!);
        await ThrowIfEmailUsed();
        
        var user = new AppUser
        {
            Email = Email.Value,
            UserName = Username.Value,
            Firstname = Username.Value!,
            Lastname = Username.Value!
        };
        
        var result = await _userManager.CreateAsync(user, Password.Value!);
        var roleResult = await _userManager.AddToRoleAsync(user, RoleConstants.SuperAdministrator);
        
        if (!result.Succeeded)
            throw new InternalServerException("Failed to create SuperAdministrator");
        
        if (!roleResult.Succeeded)
            throw new InternalServerException("Failed to add SuperAdministrator role");
    }
    
    public async Task<AppUser?> GetSuperAdministrator()
    {
        var superAdministrators = await _userManager
            .GetUsersInRoleAsync(RoleConstants.SuperAdministrator);

        return superAdministrators.FirstOrDefault();
    }

    public async Task UpdateSuperAdministrator(AppUser super)
    {
        await _userManager.RemovePasswordAsync(super);
        await _userManager.AddPasswordAsync(super, Password.Value!);

        super.Email = Email.Value;
        super.UserName = Username.Value;
        
        var result = await _userManager.UpdateAsync(super);
    
        if (!result.Succeeded)
            throw new Exception("Failed to update SuperAdministrator");
    }
    
    public async Task<bool> IsSuperAdministratorUpdated(AppUser super)
    {
        var isPasswordValid = await _userManager.CheckPasswordAsync(super, Password.Value!);
        return super.Email == Email.Value ||
               super.UserName == Username.Value ||
               !isPasswordValid; 
    }
    
    private async Task ThrowIfEmailUsed()
    {
        var user = await _userManager.FindByEmailAsync(Email.Value!);

        if (user is not null)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(RoleConstants.SuperAdministrator))
                throw new ConflictException(nameof(AppUser), Email.Value!);
        }
    }
    
    private void ThrowIfInvalidPassword(string password)
    {
        var passwordValidator = new PasswordValidator(_configuration);
        var validationResult = passwordValidator.Validate(password);
        if (validationResult.IsValid) return;
        
        throw new ValidationException("Password validation failed");
    }
}