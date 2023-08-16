using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Pakosti.Domain.Entities;

namespace Pakosti.Infrastructure.Persistence.EntityTypeConfigurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    private readonly IConfiguration _configuration;

    public CurrencyConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        var length = _configuration.GetValue<int>($"Settings:{nameof(Currency)}:Length");
        
        builder.HasKey(c => c.Name);
        builder.Property(c => c.Name).HasMaxLength(length);
    }
}