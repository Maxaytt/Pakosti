using AutoMapper;
using Pakosti.Application.Features.Products.Queries.GetProductList;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Products.Queries;

public class GetProductListQueryHandlerTests : TestCommandBase
{
    private readonly ProductContextFactory _contextFactory = new();
    private readonly IMapper _mapper;
    
    public GetProductListQueryHandlerTests()
    {
        var fixture = new QueryTestFixture();
        Context = fixture.Context;
        _mapper = fixture.Mapper;
    }
    
    [Fact]
    public async Task GetList_ValidQuery_ReturnsList()
    {
        await _contextFactory.SetUpForGettingList(Context);

        var vm = _mapper.Map<ProductListVm>(_contextFactory.Products);
        
        var handler = new GetProductListQueryHandler(Context, _mapper);
        var query = new GetProductListQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Products.ShouldNotBeNull();
        result.ShouldBeOfType<ProductListVm>();
        result.Products.Count.ShouldBe(vm.Products.Count);
    }
}