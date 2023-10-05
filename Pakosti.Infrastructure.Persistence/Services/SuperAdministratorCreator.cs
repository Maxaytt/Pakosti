using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Validators;
using Pakosti.Domain.Constants;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.Services;

public class SuperAdministratorCreator : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public SuperAdministratorCreator(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<AppUser>>();
        
        var email = _configuration[SecretKeys.SuperAdmin.Email];

        if (await IsSuperAdministratorExists())
            throw new InternalServerException("SuperAdministrator already exists");

        if (await IsEmailUsed(email!, cancellationToken))
            throw new ConflictException(nameof(AppUser), email!);
        
        var username = _configuration[SecretKeys.SuperAdmin.Username];
        var password = _configuration[SecretKeys.SuperAdmin.Password];
        ValidatePassword(password!);
        
        var user = new AppUser
        {
            Email = email,
            UserName = username,
            Firstname = username!,
            Lastname = username!
        };
        
        var result = await userManager.CreateAsync(user, password!);
        if (!result.Succeeded)
            throw new InternalServerException("Failed to create SuperAdministrator");
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task<bool> IsSuperAdministratorExists()
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<AppUser>>();
        
        var superAdministrators = await userManager
            .GetUsersInRoleAsync(RoleConstants.SuperAdministrator);

        return superAdministrators.Any();
    }

    private async Task<bool> IsEmailUsed(string email, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<AppUser>>();
        
        var users = userManager.Users;
        return await users.AnyAsync(user => user
            .Email == email, cancellationToken);
    }

    private void ValidatePassword(string password)
    {
        var passwordValidator = new PasswordValidator(_configuration);
        var validationResult = passwordValidator.Validate(password);
        if (validationResult.IsValid) return;
        
        throw new ValidationException("Password validation failed");
    }
}