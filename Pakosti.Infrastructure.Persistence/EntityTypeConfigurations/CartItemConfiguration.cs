using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Product)
            .WithMany()
            .HasForeignKey(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(c => c.CartId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(c => new { c.CartId, ProductId = c.Id }).IsUnique();
    }
}