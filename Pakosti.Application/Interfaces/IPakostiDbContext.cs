using Microsoft.EntityFrameworkCore;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Interfaces;

public interface IPakostiDbContext : ICategoryNullSetter
{
    DbSet<Category> Categories { get; set; }
    DbSet<Cart> Carts { get; set; }
    DbSet<CartItem> CartItems { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Price> Prices { get; set; }
    DbSet<Review> Reviews { get; set; }
    DbSet<Currency> Currencies { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}