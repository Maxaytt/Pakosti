using AutoMapper;
using Moq;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Reviews.Queries;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Reviews.Queries;

public class GetReviewQueryHandlerTests : TestCommandBase
{
    private readonly ReviewContextFactory _contextFactory = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task GetReview_Success()
    {
        // Arrange
        await _contextFactory.SetUpForGetting(Context);

        var vm = new GetReview.Response(
            _contextFactory.ReviewForGet.Id,
            _contextFactory.ReviewForGet.ProductId,
            _contextFactory.ReviewForGet.Header,
            _contextFactory.ReviewForGet.Body,
            DateTime.Now,
            null);

        _mapperMock.Setup(x => x.Map<GetReview.Response>(It.IsAny<Review>()))
            .Returns(vm);

        var handler = new GetReview.Handler(Context, _mapperMock.Object);
        var query = new GetReview.Query(_contextFactory.ReviewForGet.Id);
        
        // Act 
        var category = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        category.ShouldNotBeNull();
        category.Id.ShouldBe(query.Id);
        category.ProductId.ShouldBe(_contextFactory.ReviewForGet.ProductId);
        category.Header.ShouldBe(_contextFactory.ReviewForGet.Header);
        category.Body.ShouldBe(_contextFactory.ReviewForGet.Body);
        category.EditionDate.ShouldBe(_contextFactory.ReviewForGet.EditionDate);
    }

    [Fact]
    public async Task GetReview_ReviewNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var vm = new GetReview.Response(
            Guid.NewGuid(),
            _contextFactory.ReviewForGet.ProductId,
            _contextFactory.ReviewForGet.Header,
            _contextFactory.ReviewForGet.Body,
            DateTime.Now,
            null);
        
        _mapperMock.Setup(x => x.Map<GetReview.Response>(It.IsAny<Review>()))
            .Returns(vm);
        
        var handler = new GetReview.Handler(Context, _mapperMock.Object);
        var query = new GetReview.Query(_contextFactory.ReviewForGet.Id);
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(query, CancellationToken.None));
        exception.EntityId.ShouldBe(query.Id);
        exception.EntityName.ShouldBe(nameof(Review));
    }
}