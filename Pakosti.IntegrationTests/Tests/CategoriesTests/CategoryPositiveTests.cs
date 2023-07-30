using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Categories.Queries;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.CategoriesTests;

public class CategoryPositiveTests
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateCategory_ValidRequest(HttpClient client)
    {
        //Arrange
        var request = new CreateCategory.Command(Guid.NewGuid(), "TestCategory");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/categories/createcategory", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        request.ParentCategoryId.ShouldNotBeNull();
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteCategory_ValidRequest(HttpClient client)
    {
        //Arrange
        var request = new DeleteCategory.Command(Guid.NewGuid());
        
        // Act
        var response = await client.DeleteAsync("/api/categories/deletecategory");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateCategory_ValidRequest(HttpClient client)
    {
        //Arrange
        var request = new UpdateCategory.Command(Guid.NewGuid(),Guid.NewGuid() ,"UpdatedCategoryName");
        
        // Act
        var response = await client.PutAsJsonAsync("/api/categories/updatecategory", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        request.ParentCategoryId.ShouldNotBeNull();
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategory_ValidRequest(HttpClient client)
    {
        //Arrange
        var request = new GetCategory.Query(Guid.NewGuid());
        
        // Act
        var response = await client.GetAsync("/api/categories/getcategory");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategoryList_ValidRequest(HttpClient client)
    {
        // Act
        var response = await client.GetAsync("/api/categories");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}