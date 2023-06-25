using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Cqrs.Categories.Queries.GetCategoryList;
using Pakosti.Application.Cqrs.Products.Queries.GetProductList;
using Pakosti.Application.Cqrs.Reviews.Queries.GetReviewList;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Tests.Common;

using AutoMapper;
using Xunit;

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
            config.CreateMap<List<Product>, ProductListVm>()
                .ForMember(p => p.Products,
                    opt => opt.MapFrom(l => l));
            config.CreateMap<List<Category>, CategoryListVm>()
                .ForMember(c => c.Categories,
                    opt => opt.MapFrom(l => l));
            config.CreateMap<List<Review>, ReviewListVm>()
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
