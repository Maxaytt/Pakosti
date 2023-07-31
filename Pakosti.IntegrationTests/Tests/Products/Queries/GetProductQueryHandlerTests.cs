using AutoMapper;
using Moq;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Products.Queries;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Products.Queries;

public class GetProductQueryHandlerTests : TestCommandBase
{
    private readonly ProductContextFactory _contextFactory = new();
    private readonly Mock<IMapper> _mapper = new();

    [Fact]
    public async Task Get_ValidQuery_ReturnsProduct()
    {
        // Arrange
        await _contextFactory.SetUpForGetting(Context);

        var vm = new GetProduct.Response(
            _contextFactory.ProductForGetting.Id,
            _contextFactory.ProductForGetting.CategoryId,
            null,
            _contextFactory.ProductForGetting.Name,
            _contextFactory.ProductForGetting.Description,
            _contextFactory.ProductForGetting.CreationDate,
            _contextFactory.ProductForGetting.EditionDate);
        
        _mapper.Setup(x => x.Map<GetProduct.Response>(It.IsAny<Product>()))
            .Returns(vm);
        
        var handler = new GetProduct.Handler(Context, _mapper.Object);
        var query = new GetProduct.Query(_contextFactory.ProductForGetting.Id);
        
        // Act
        var product = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        product.ShouldNotBeNull();
        product.Id.ShouldBe(query.Id);
    }

    [Fact]
    public async Task Get_ProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForGetting(Context);

        var vm = new GetProduct.Response(
            _contextFactory.ProductForGetting.Id,
            _contextFactory.ProductForGetting.CategoryId,
            null,
            _contextFactory.ProductForGetting.Name,
            _contextFactory.ProductForGetting.Description,
            _contextFactory.ProductForGetting.CreationDate,
            _contextFactory.ProductForGetting.EditionDate);
        
        _mapper.Setup(x => x.Map<GetProduct.Response>(It.IsAny<Product>()))
            .Returns(vm);
        
        var handler = new GetProduct.Handler(Context, _mapper.Object);
        var query = new GetProduct.Query(Guid.NewGuid());
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(query, CancellationToken.None));
        exception.EntityName.ShouldBe(nameof(Product));
        exception.EntityId.ShouldBe(query.Id);
    }
}