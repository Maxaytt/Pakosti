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

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(review => review.ProductId);//TODO: OnDelete Cascade
        
        builder.Property(review => review.Header).HasMaxLength(100);
        builder.Property(review => review.Body).HasMaxLength(2000);
    }
}