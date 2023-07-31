using Pakosti.Infrastructure.Persistence;

namespace Pakosti.IntegrationTests.Common;

public class QueryTestFixture : IDisposable
{
    public readonly PakostiDbContext Context;

    public QueryTestFixture()
    {
        Context = ContextFactory.Create();
    }

    public void Dispose()
    {
        ContextFactory.Destroy(Context);
    }
}
