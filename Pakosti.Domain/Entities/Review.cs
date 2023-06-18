namespace Pakosti.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    
    public string Header { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public DateTime? EditionDate { get; set; }
}