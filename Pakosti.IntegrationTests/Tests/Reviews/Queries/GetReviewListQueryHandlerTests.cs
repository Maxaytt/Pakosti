using AutoMapper;
using Pakosti.Application.Features.Reviews.Queries.GetReviewList;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Reviews.Queries;

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