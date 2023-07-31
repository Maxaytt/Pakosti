using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Features.Categories.Queries;
using Pakosti.Application.Features.Products.Queries;
using Pakosti.Application.Features.Reviews.Queries;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.IntegrationTests.Common;

public class QueryTestFixture : IDisposable
{
    public readonly PakostiDbContext Context;
    public readonly IMapper Mapper;

    public QueryTestFixture()
    {
        Context = ContextFactory.Create();
        var configurationProvider = new MapperConfiguration(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(
                typeof(IPakostiDbContext).Assembly));
            config.CreateMap<List<Product>, GetProductList.Response>()
                .ForMember(p => p.Products,
                    opt => opt.MapFrom(l => l));
            config.CreateMap<List<Category>, GetCategoryList.Response>()
                .ForMember(c => c.Categories,
                    opt => opt.MapFrom(l => l));
            config.CreateMap<List<Review>, GetReviewList.Response>()
                .ForMember(r => r.Reviews,
                    opt => opt.MapFrom(l => l));
        });
        Mapper = configurationProvider.CreateMapper();
    }

    public void Dispose()
    {
        ContextFactory.Destroy(Context);
    }
}
