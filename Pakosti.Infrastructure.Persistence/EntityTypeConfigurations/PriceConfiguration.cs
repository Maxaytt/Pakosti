using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

public class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.HasIndex(p => new { p.ProductId, p.CurrencyName })
            .IsUnique();
    }
}