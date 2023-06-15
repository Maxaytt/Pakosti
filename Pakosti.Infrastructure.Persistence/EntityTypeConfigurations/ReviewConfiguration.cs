using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(review => review.Id);
        builder.HasIndex(review => review.Id);

        builder.HasOne(review => review.Product)
            .WithMany(review => review.Reviews)
            .HasForeignKey(review => review.ProductId);

        builder.Property(review => review.Name).HasMaxLength(50);
        builder.Property(review => review.Header).HasMaxLength(100);
        builder.Property(review => review.Body).HasMaxLength(2000);
    }
}