using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Reviews.Commands.UpdateReview;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Reviews.Commands;

public class UpdateReviewCommandHandlerTests : TestCommandBase
{
    private readonly ReviewContextFactory _contextFactory = new();

    [Fact]
    public async Task Update_ValidCommand_ProductUpdated()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);

        var handler = new UpdateReviewCommandHandler(Context);
        var command = new UpdateReviewCommand
        {
            Id = _contextFactory.ReviewIdForUpdate,
            Header = "test review",
            Body = "its ok",
            UserId = ContextFactory.UserAId
        };
        
        // Act 
        await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var updatedReview = await Context.Reviews
            .FirstOrDefaultAsync(r => r.Id == command.Id, CancellationToken.None);
        updatedReview.ShouldNotBeNull();
        updatedReview.Body.ShouldBe(command.Body);
        updatedReview.Header.ShouldBe(command.Header);
        updatedReview.EditionDate.ShouldNotBeNull();
    }

    [Fact]
    public async Task Update_InvalidReviewId_ThrowsNotFoundException()
    {
        // Arrange
        var handler = new UpdateReviewCommandHandler(Context);
        var command = new UpdateReviewCommand
        {
            Id = Guid.NewGuid(),
            Header = "test review",
            Body = "wrong review",
            UserId = ContextFactory.UserAId
        };
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(command, CancellationToken.None));
        exception.EntityId.ShouldBe(command.Id);
        exception.EntityName.ShouldBe(nameof(Review));
    }
}