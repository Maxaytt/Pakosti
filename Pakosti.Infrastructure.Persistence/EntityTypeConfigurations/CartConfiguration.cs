using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.UserId);

        builder.HasMany(c => c.CartItems)
            .WithOne(c => c.Cart)
            .HasForeignKey(c => c.CartId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<AppUser>()
            .WithOne()
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}