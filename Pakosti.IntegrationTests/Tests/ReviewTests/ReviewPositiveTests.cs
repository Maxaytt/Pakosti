using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Consumer.Reviews.Commands;
using Pakosti.Application.Features.Guest.Reviews.Queries;
using Pakosti.IntegrationTests.Attributes;
using Pakosti.IntegrationTests.Services;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.ReviewTests;

public class ReviewPositiveTests
{
    private const string Name = "test test";
    private const string Description = "test test test test!";
    private const string Header = "test test";
    private const string UpdatedHeader = "updated test header";
    private const string Body = "test test test test test!";
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateReview_ShouldCreate_Review(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, 
            (categoryId, Name, Description));
        var request = new CreateReview.Dto(productId, Header, Body);
        
        var response = await client.PostAsJsonAsync("/api/review", request);
        var responseData = await response.Content.ReadFromJsonAsync<CreateReview.Response>();
        var review = await TestRequestService.GetReview(client, responseData!.Id);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        review!.Header.ShouldBe(Header);
        review.Body.ShouldBe(Body);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateReview_ShouldUpdate_Review(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, (categoryId, Name, Description));
        var reviewId = await TestRequestService.CreateReview(client, (productId, Header, Body));
        var request = new UpdateReview.Dto(reviewId, UpdatedHeader, null);
        
        var response = await client.PutAsJsonAsync("/api/review", request);
        var product = await TestRequestService.GetReview(client, reviewId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        product!.Header.ShouldBe(UpdatedHeader);
        product.Body.ShouldBe(Body);
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteReview_ShouldDelete_Review(HttpClient client)
    {
         
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, (categoryId, Name, Description));
        var reviewId = await TestRequestService.CreateReview(client, (productId, Header, Body));
        
        var response = await client.DeleteAsync($"/api/review/{reviewId}");
        var review = await TestRequestService.GetReview(client, reviewId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        review.ShouldBeNull();
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task GetReview_ShouldGet_Review(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, (categoryId, Name, Description));
        var reviewId = await TestRequestService.CreateReview(client, (productId, Header, Body));
        
        var response = await client.GetAsync($"/api/review/{reviewId}");
        var responseData = await response.Content.ReadFromJsonAsync<GetReview.Response>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseData!.Id.ShouldBe(reviewId);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetReviewList_ShouldGetAll_Reviews(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var categoryId = await TestRequestService.CreateCategory(client, (null, Name));
        var productId = await TestRequestService.CreateProduct(client, (categoryId, Name, Description));
        var reviewIds = new List<Guid>
        {
            await TestRequestService.CreateReview(client, (productId, Header + "1", Body)),
            await TestRequestService.CreateReview(client, (productId, Header + "2", Body))
        };
        
        var response = await client.GetAsync("/api/review");
        var responseData = await response.Content.ReadFromJsonAsync<GetReviewList.Response>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseData!.Reviews.ToList()
            .ForEach(r => reviewIds.ShouldContain(r.Id));
    }
}