namespace Pakosti.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? CategoryId { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? EditionDate { get; set; }
}