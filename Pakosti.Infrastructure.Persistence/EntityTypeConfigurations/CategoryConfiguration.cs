using Pakosti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);
        builder.HasIndex(category => category.Id);
        builder.HasMany<Product>()
            .WithOne()
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(category => category.ParentCategoryId);

        builder.Property(category => category.Name).HasMaxLength(50);
    }
}