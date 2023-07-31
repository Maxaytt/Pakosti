using AutoMapper;
using Pakosti.Application.Features.Categories.Queries;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Categories.Queries;

public class GetCategoryListQueryHandlerTests : TestCommandBase
{
    private readonly CategoryContextFactory _contextFactory = new();
    private readonly IMapper _mapper;

    public GetCategoryListQueryHandlerTests()
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

        _mapper.Map<GetCategoryList.Response>(_contextFactory.Categories);

        var handler = new GetCategoryList.Handler(Context, _mapper);
        var query = new GetCategoryList.Query();
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<GetCategoryList.Response>();
        result.Categories.Count.ShouldBe(_contextFactory.Categories.Count);
    }
}