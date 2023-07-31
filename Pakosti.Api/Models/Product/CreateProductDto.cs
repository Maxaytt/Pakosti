namespace Pakosti.Api.Models.Product;

public sealed record CreateProductDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}