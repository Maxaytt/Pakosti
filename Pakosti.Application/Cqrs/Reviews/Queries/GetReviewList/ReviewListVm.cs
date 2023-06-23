namespace Pakosti.Application.Cqrs.Reviews.Queries.GetReviewList;

public class ReviewListVm
{
    public IList<ReviewLookupDto> Reviews { get; set; } = null!;
}