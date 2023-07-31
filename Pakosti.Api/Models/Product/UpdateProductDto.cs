namespace Pakosti.Api.Models.Product;

public sealed record UpdateProductDto 
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string? Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
}