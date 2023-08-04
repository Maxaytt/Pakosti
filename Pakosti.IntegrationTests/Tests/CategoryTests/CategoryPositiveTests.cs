using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Categories.Queries;
using Pakosti.IntegrationTests.Attributes;
using Pakosti.IntegrationTests.Services;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.CategoryTests;

public class CategoryPositiveTests
{

    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateCategory_ValidRequest_ReturnsOk(HttpClient client)
    {
        //Arrange
        await TestDataInitializer.RegisterUser(client);
        var request = new CreateCategory.Command(null, "TestCategory");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/category", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteCategory_ValidRequest(HttpClient client)
    {
        //Arrange
        await TestDataInitializer.RegisterUser(client);
        var id = await TestDataInitializer.CreateCategory(client, null, "test1");
        var childId = await TestDataInitializer.CreateCategory(client, id, "test2");
        

        // Act
        var response = await client.DeleteAsync($"/api/category/{id}");
        var childResponse = await client.GetAsync($"/api/category/{childId}");

        // Assert
        var childData = await childResponse.Content.ReadFromJsonAsync<GetCategory.Response>();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        childData!.ParentCategoryId.ShouldBeNull();
        
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateCategory_ValidRequest(HttpClient client)
    {
        //Arrange
        await TestDataInitializer.RegisterUser(client);
        var id = await TestDataInitializer.CreateCategory(client, null, "test");
        var request = new UpdateCategory.Command(id, null,"UpdatedCategoryName");
        
        // Act
        var response = await client.PutAsJsonAsync("/api/category", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategory_ValidRequest(HttpClient client)
    {
        //Arrange
        await TestDataInitializer.RegisterUser(client);
        var id = await TestDataInitializer.CreateCategory(client, null, "test");

        // Act
        var response = await client.GetAsync($"/api/category/{id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategoryList_ValidRequest(HttpClient client)
    {
        // Act
        await TestDataInitializer.RegisterUser(client);
        await TestDataInitializer.CreateCategory(client, null, "test");

        var response = await client.GetAsync("/api/category");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}