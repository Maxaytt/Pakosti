using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Tests.Common;

public class TestCommandBase : IDisposable
{
    protected readonly PakostiDbContext Context;

    public TestCommandBase()
    {
        Context = ContextFactory.Create();
    }


    public void Dispose()
    {
        ContextFactory.Destroy(Context);
    }
}