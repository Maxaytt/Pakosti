using AutoMapper;
using Pakosti.Application.Cqrs.Categories.Queries.GetCategoryList;
using Pakosti.Tests.Common;
using Pakosti.Tests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.Tests.Cqrs.Categories.Queries;

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

        _mapper.Map<CategoryListVm>(_contextFactory.Categories);

        var handler = new GetCategoryListQueryHandler(Context, _mapper);
        var query = new GetCategoryListQuery();
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<CategoryListVm>();
        result.Categories.Count.ShouldBe(_contextFactory.Categories.Count);
    }
}