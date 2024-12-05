using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MostIdea.MIMGroup.EntityFrameworkCore;

namespace MostIdea.MIMGroup.HealthChecks
{
    public class MIMGroupDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public MIMGroupDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("MIMGroupDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("MIMGroupDbContext could not connect to database"));
        }
    }
}
