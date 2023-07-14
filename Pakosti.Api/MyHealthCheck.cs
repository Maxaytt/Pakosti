using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Pakosti.Api
{
    public class MyHealthCheck : IHealthCheck
    {
        private readonly int _arg1;
        private readonly string _arg2;

        public MyHealthCheck(int arg1, string arg2)
        {
            _arg1 = arg1;
            _arg2 = arg2;
        }

        public bool IsHealthy { get; set; } = true;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (IsHealthy)
            {
                return HealthCheckResult.Healthy("A healthy result.");
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return await Task.FromResult(new HealthCheckResult(
                context.Registration.FailureStatus, "An unhealthy result."));
        }
    }
}