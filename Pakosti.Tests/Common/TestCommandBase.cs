using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Tests.Common;

public class TestCommandBase : IDisposable
{
    protected PakostiDbContext Context;

    public TestCommandBase()
    {
        Context = ContextFactory.Create();
        Context.IsDisposed = false;
    }


    public void Dispose()
    {
        ContextFactory.Destroy(Context);
        Context.IsDisposed = true;
    }
}