using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pakosti.Infrastructure.Persistence.Services;

public class SuperAdministratorCreator : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public SuperAdministratorCreator(IServiceScopeFactory scopeFactory) =>
        _scopeFactory = scopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider
            .GetRequiredService<SuperAdministratorService>();
        
        var superAdmin = await service.GetSuperAdministrator();
        
        if (superAdmin is null)
        {
            await service.CreateSuperAdministrator(cancellationToken);
        } 
        else if (await service.IsSuperAdministratorUpdated(superAdmin))
        {
            await service.UpdateSuperAdministrator(superAdmin);
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}