using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.Application.Features.Products.Queries;
using Pakosti.IntegrationTests.Attributes;
using Pakosti.IntegrationTests.Services;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.ProductTests;

public class ProductPositiveTests
{
    private const string UpdatedName = "updated test";
    private const string Name = "test test";
    private const string Description = "test test test test!";
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateProduct_ShouldCreate_Product(HttpClient client)
    {
        // Arrange
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var request = new CreateProduct.Dto(categoryId, Name, Description);
        
        // Act
        var response = await client.PostAsJsonAsync("/api/product", request);
        var responseData = await response.Content.ReadFromJsonAsync<CreateProduct.Response>();
        var product = await TestRequestService.GetProduct(client, responseData!.Id);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        product!.Name.ShouldBe(Name);
        product.Description.ShouldBe(Description);
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateProduct_ShouldUpdate_Product(HttpClient client)
    {
        // Arrange
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, (categoryId, Name, Description));
        var request = new UpdateProduct.Dto(productId, null, UpdatedName, null);
        
        // Act
        var response = await client.PutAsJsonAsync("/api/product", request);
        var product = await TestRequestService.GetProduct(client, productId);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        product!.Name.ShouldBe(UpdatedName);
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteProduct_ShouldDelete_Product(HttpClient client)
    {
        // Arrange 
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, (categoryId, Name, Description));
        
        // Act
        var response = await client.DeleteAsync($"/api/product/{productId}");
        var product = await TestRequestService.GetProduct(client, productId);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        product.ShouldBeNull();
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task GetProduct_ShouldGet_Product(HttpClient client)
    {
        // Arrange
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, (categoryId, Name, Description));
        
        // Act
        var response = await client.GetAsync($"/api/product/{productId}");
        var responseData = await response.Content.ReadFromJsonAsync<GetProduct.Response>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseData!.Id.ShouldBe(productId);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetProduct_ShouldGetAll_Products(HttpClient client)
    {
        // Arrange
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productIds = new List<Guid>
        {
            await TestRequestService.CreateProduct(client, (categoryId, Name + "1", Description)),
            await TestRequestService.CreateProduct(client, (categoryId, Name + "2", Description))
        };
        
        // Act
        var response = await client.GetAsync("/api/product");
        var responseData = await response.Content.ReadFromJsonAsync<GetProductList.Response>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseData!.Products.ToList()
            .ForEach(p => productIds.ShouldContain(p.Id));
    }
}