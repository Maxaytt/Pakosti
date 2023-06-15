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
        builder.HasMany(category => category.Products)
            .WithOne(product => product.Category)
            .HasForeignKey(product => product.CategoryId);
        
        builder.HasMany(category => category.SubCategories)
            .WithOne(category => category.ParentCategory)
            .HasForeignKey(category => category.ParentCategoryId);
        
        builder.Property(category => category.Name).HasMaxLength(50);
    }
}