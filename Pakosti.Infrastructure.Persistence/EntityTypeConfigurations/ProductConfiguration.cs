using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);
        builder.HasIndex(product => product.Id);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany<Review>()
            .WithOne()
            .HasForeignKey(review => review.ProductId);

        builder.Property(product => product.Name).HasMaxLength(50);
        builder.Property(product => product.Description).HasMaxLength(2000);
    }
}