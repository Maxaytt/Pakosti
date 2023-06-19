using Microsoft.EntityFrameworkCore;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Tests.Common;

public abstract class ContextFactory
{
    public Guid UserAId = Guid.NewGuid();
    public Guid UserBId = Guid.NewGuid();
    
    public static PakostiDbContext Create()
    {
        var options = new DbContextOptionsBuilder<PakostiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new PakostiDbContext(options);
        context.Database.EnsureCreated();
        context.SaveChanges();
        return context;
    }

    public static void Destroy(PakostiDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}