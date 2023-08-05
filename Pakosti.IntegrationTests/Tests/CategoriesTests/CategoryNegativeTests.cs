using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Categories.Queries;
using Pakosti.Application.Features.Identities.Commands;
using Pakosti.IntegrationTests.Attributes;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.CategoriesTests;

public class CategoryNegativeTests
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateCategory_InvalidRequest(HttpClient client)
    {
        //Arrange
        var request = new CreateCategory.Command(null, "Short");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/categories/createcategory", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteCategory_InvalidRequest(HttpClient client)
    {
        // Act
        var response = await client.DeleteAsync("/api/categories/deletecategory");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateCategory_InvalidRequest(HttpClient client)
    {
        //Arrange
        var request = new UpdateCategory.Command(Guid.NewGuid(),null ,"UpdatedCategoryName");
        
        // Act
        var response = await client.PutAsJsonAsync("/api/categories/updatecategory", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategory_InvalidRequest(HttpClient client)
    {
        // Act
        var response = await client.GetAsync("/api/categories/getcategory");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategoryList_ValidRequest(HttpClient client)
    {
        // Act
        var response = await client.GetAsync("/api/categories");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}