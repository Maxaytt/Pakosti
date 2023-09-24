using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

namespace Pakosti.Infrastructure.Persistence;

public class PakostiDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IPakostiDbContext 
{
    public bool IsDisposed { get; set; }
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Price> Prices { get; set; }
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; }

    public PakostiDbContext(DbContextOptions<PakostiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    
    public async Task SetNullCategoryChildes(Category entity)
    {
        var categories = Categories.Where(c => c.ParentCategoryId == entity.Id);
        await categories.ForEachAsync(c => c.ParentCategoryId = null);
        await SaveChangesAsync();
    }
}