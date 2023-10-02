using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

namespace Pakosti.Infrastructure.Persistence;

public class PakostiDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IPakostiDbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Price> Prices { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; } = null!;

    public PakostiDbContext(DbContextOptions<PakostiDbContext> options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CartConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyConfiguration(_configuration));
        modelBuilder.ApplyConfiguration(new PriceConfiguration());
        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    
    public async Task SetNullCategoryChildes(Category entity)
    {
        var categories = Categories.Where(c => c.ParentCategoryId == entity.Id);
        await categories.ForEachAsync(c => c.ParentCategoryId = null);
        await SaveChangesAsync();
    }
}