using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Testcontainers.PostgreSql;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.CategoriesTests;

public class CategoryNegativeTests
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateCategory_InvalidRequest(HttpClient client)
    {
        //Arrange
        var request = new CreateCategory.Command(null, "test");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/category", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteCategory_InvalidRequest(HttpClient client)
    {
        // Act
        var id = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/category/{id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateCategory_InvalidRequest(HttpClient client)
    {
        //Arrange
        var request = new UpdateCategory.Command(Guid.NewGuid(),null ,"UpdatedCategoryName");
        
        // Act
        var response = await client.PutAsJsonAsync("/api/category", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategory_InvalidRequest(HttpClient client)
    {
        // Act
        var id = Guid.NewGuid();
        var response = await client.GetAsync($"/api/categories/{id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategoryList_ValidRequest(HttpClient client, PostgreSqlContainer container)
    {
        // Arrange
        await container.StopAsync();
        
        // Act
        var response = await client.GetAsync("/api/category");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }
}