namespace Pakosti.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }

    public Product Product { get; set; } = null!;
    public Cart Cart { get; set; } = null!;
    
    public decimal CostOfOne { get; set; }
    public int Amount { get; set; }

    public decimal TotalCost => CostOfOne * Amount;
}