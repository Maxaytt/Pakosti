namespace Pakosti.Domain.Entities;

public class Currency
{
    public string Name { get; set; } = null!;
    public decimal Coefficient { get; set; }
}