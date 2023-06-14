namespace Pakosti.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public DateTime? EditionDate { get; set; }
    public List<Review> Reviews { get; set; } = null!;
}