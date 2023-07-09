using Microsoft.EntityFrameworkCore;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.IntegrationTests.Common;

public abstract class ContextFactory
{
    public static Guid UserAId = Guid.NewGuid();
    public static Guid UserBId = Guid.NewGuid();
    
    public static PakostiDbContext Create()
    {
        var options = new DbContextOptionsBuilder<PakostiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new PakostiDbContext(options);
        context.Database.EnsureCreated();
        context.SaveChanges();
        context.IsDisposed = false;
        return context;
    }

    public static void Destroy(PakostiDbContext context)
    {
        if (context is not { IsDisposed: false }) return;
        if (context.Database.ProviderName != null)
        {
            if (!context.Database.ProviderName.Contains("InMemory"))
            {
                context.Database.EnsureDeleted();
            }
        }
        context.Dispose();
        context.IsDisposed = true;
    }
}