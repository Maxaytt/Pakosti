using Microsoft.EntityFrameworkCore;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Interfaces;

public interface IPakostiDbContext : ICategoryNullSetter
{
    DbSet<Category> Categories { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Review> Reviews { get; set; }
    DbSet<Currency> Currencies { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}