namespace Pakosti.Api.Models.Review;

public sealed record CreateReviewDto 
{
    public Guid ProductId { get; set; }
    public string Header { get; set; } = null!;
    public string Body { get; set; } = null!;
}