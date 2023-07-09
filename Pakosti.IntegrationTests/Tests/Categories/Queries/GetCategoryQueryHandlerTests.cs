using AutoMapper;
using Moq;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Cqrs.Categories.Queries.GetCategory;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Categories.Queries;

public class GetCategoryQueryHandlerTests : TestCommandBase
{
    private readonly CategoryContextFactory _contextFactory = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task GetCategory_Success()
    {
        // Arrange
        await _contextFactory.SetUpForGetting(Context);

        var vm = new CategoryVm
        {
            Id = _contextFactory.CategoryForGet.Id,
            Name = _contextFactory.CategoryForGet.Name,
            ParentCategoryId = _contextFactory.CategoryForGet.ParentCategoryId
        };

        _mapperMock.Setup(x => x.Map<CategoryVm>(It.IsAny<Category>()))
            .Returns(vm);

        var handler = new GetCategoryQueryHandler(Context, _mapperMock.Object);
        var query = new GetCategoryQuery
        {
            Id = _contextFactory.CategoryForGet.Id
        };
        
        // Act 
        var category = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        category.ShouldNotBeNull();
        category.Id.ShouldBe(query.Id);
        category.Name.ShouldBe(_contextFactory.CategoryForGet.Name);
        category.ParentCategoryId.ShouldBe(_contextFactory.CategoryForGet.ParentCategoryId);
    }

    [Fact]
    public async Task GetCategory_CategoryNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var vm = new CategoryVm
        {
            Id = _contextFactory.CategoryForGet.Id,
            Name = "wrong category",
            ParentCategoryId = null
        };
        _mapperMock.Setup(x => x.Map<CategoryVm>(It.IsAny<Category>()))
            .Returns(vm);
        
        var handler = new GetCategoryQueryHandler(Context, _mapperMock.Object);
        var query = new GetCategoryQuery
        {
            Id = _contextFactory.CategoryForGet.Id
        };
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(query, CancellationToken.None));
        exception.EntityId.ShouldBe(query.Id);
        exception.EntityName.ShouldBe(nameof(Category));
    }
}