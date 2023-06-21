using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Cqrs.Products.Queries.GetProductList;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence;
using Pakosti.Tests.Common.CqrsFactories;

namespace Pakosti.Tests.Common;

using AutoMapper;
using Xunit;

public class QueryTestFixture : IDisposable
{
    public PakostiDbContext Context;
    public IMapper Mapper;

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
        });
        Mapper = configurationProvider.CreateMapper();
    }

    public void Dispose()
    {
        ContextFactory.Destroy(Context);
    }
}

[CollectionDefinition("QueryCollection")]
public class QueryCollection : ICollectionFixture<QueryTestFixture>
{
    
}