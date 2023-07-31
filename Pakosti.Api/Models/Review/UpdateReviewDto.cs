namespace Pakosti.Api.Models.Review;

public sealed record UpdateReviewDto 
{
    public Guid Id { get; set; }
    public string? Header { get; set; }
    public string? Body { get; set; }
}