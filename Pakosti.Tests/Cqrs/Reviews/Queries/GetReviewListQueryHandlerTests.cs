using AutoMapper;
using Pakosti.Application.Cqrs.Reviews.Queries.GetReviewList;
using Pakosti.Tests.Common;
using Pakosti.Tests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.Tests.Cqrs.Reviews.Queries;

public class GetReviewListQueryHandlerTests : TestCommandBase
{
    private readonly ReviewContextFactory _contextFactory = new();
    private readonly IMapper _mapper;

    public GetReviewListQueryHandlerTests()
    {
        var fixture = new QueryTestFixture();
        _mapper = fixture.Mapper;
        Context = fixture.Context;
    }

    [Fact]
    public async Task GetList_ValidQuery_ReturnsList()
    {
        // Arrange
        await _contextFactory.SetUpForGettingList(Context);

        _mapper.Map<ReviewListVm>(_contextFactory.Reviews);

        var handler = new GetReviewListQueryHandler(Context, _mapper);
        var query = new GetReviewListQuery();
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<ReviewListVm>();
        result.Reviews.Count.ShouldBe(_contextFactory.Reviews.Count);
    }
}