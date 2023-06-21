using AutoMapper;
using Moq;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Cqrs.Products.Queries.GetProduct;
using Pakosti.Application.Cqrs.Products.Queries.GetProductList;
using Pakosti.Domain.Entities;
using Pakosti.Tests.Common;
using Pakosti.Tests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.Tests.Products.Queries;

public class GetProductQueryHandlerTests : TestCommandBase
{
    private readonly ProductContextFactory _contextFactory = new();
    private readonly Mock<IMapper> _mapper = new();

    [Fact]
    public async Task GetProduct_Success()
    {
        // Arrange
        await _contextFactory.SetUpForGetting(Context);

        var vm = new ProductVm
        {
            Id = _contextFactory.ProductForGetting.Id,
            CategoryId = _contextFactory.ProductForGetting.CategoryId,
            CategoryName = null,
            Name = _contextFactory.ProductForGetting.Name,
            Description = _contextFactory.ProductForGetting.Description,
            CreationDate = _contextFactory.ProductForGetting.CreationDate,
            EditionDate = _contextFactory.ProductForGetting.EditionDate
        };
        
        _mapper.Setup(x => x.Map<ProductVm>(It.IsAny<Product>()))
            .Returns(vm);
        
        var handler = new GetProductQueryHandler(Context, _mapper.Object);
        var query = new GetProductQuery
        {
            Id = _contextFactory.ProductForGetting.Id
        };
        
        // Act
        var product = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        product.ShouldNotBeNull();
        product.Id.ShouldBe(query.Id);
    }

    [Fact]
    public async Task GetProduct_ProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForGetting(Context);

        var vm = new ProductVm
        {
            Id = Guid.NewGuid(),
            CategoryId = _contextFactory.ProductForGetting.CategoryId,
            CategoryName = null,
            Name = _contextFactory.ProductForGetting.Name,
            Description = _contextFactory.ProductForGetting.Description,
            CreationDate = _contextFactory.ProductForGetting.CreationDate,
            EditionDate = _contextFactory.ProductForGetting.EditionDate
        };
        
        _mapper.Setup(x => x.Map<ProductVm>(It.IsAny<Product>()))
            .Returns(vm);
        
        var handler = new GetProductQueryHandler(Context, _mapper.Object);
        var query = new GetProductQuery
        {
            Id = _contextFactory.ProductForGetting.Id
        };
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(() =>
            handler.Handle(query, CancellationToken.None));
        exception.EntityName.ShouldBe(nameof(Product));
        exception.EntityId.ShouldBe(query.Id);
    }
}