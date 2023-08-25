namespace Pakosti.Domain.Entities;

public class Price
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string CurrencyName { get; set; } = null!;

    public decimal Cost { get; set; }
    public Currency Currency { get; set; } = null!;
}