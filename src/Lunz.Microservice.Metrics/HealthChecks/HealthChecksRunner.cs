using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lunz.Microservice.Metrics
{
    public sealed class HealthChecksRunner : IRunHealthChecks
    {
        private static readonly ILog Logger = LogProvider.For<HealthChecksRunner>();
        private readonly IEnumerable<HealthCheck> _checks;
        private readonly IMetrics _metrics;
        /// <summary>
        ///  Initializes a new instance of the <see cref="HealthChecksRunner" /> class.
        /// </summary>
        /// <param name="checks">The registered health checks.</param>
        public HealthChecksRunner(IEnumerable<HealthCheck> checks, IMetrics metrics)
        {
            _checks = checks ?? Enumerable.Empty<HealthCheck>();
            _metrics = metrics;
        }

        /// <inheritdoc />
        public async ValueTask<HealthStatus> ReadAsync(CancellationToken cancellationToken = default)
        {
            if (!_checks.Any())
            {
                return default;
            }

            var startTimestamp = Logger.IsTraceEnabled() ? Stopwatch.GetTimestamp() : 0;

            Logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(_checks.OrderBy(v => v.Name).Select(v => v.ExecuteAsync(cancellationToken).AsTask()));
            var failed = new List<HealthCheck.Result>();
            var degraded = new List<HealthCheck.Result>();

            foreach (var result in results)
            {
                var tags = new MetricTags(HealthChecksMetricsRegistry.TagKeys.HealthCheckName, result.Name);

                if (result.Check.Status == HealthCheckStatus.Degraded)
                {
                    degraded.Add(result);
                    _metrics.Measure.Gauge.SetValue(HealthChecksMetricsRegistry.Checks, tags, HealthConstants.HealthScore.degraded);
                }
                else if (result.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    failed.Add(result);
                    _metrics.Measure.Gauge.SetValue(HealthChecksMetricsRegistry.Checks, tags, HealthConstants.HealthScore.unhealthy);
                }
                else if (result.Check.Status == HealthCheckStatus.Healthy)
                {
                    _metrics.Measure.Gauge.SetValue(HealthChecksMetricsRegistry.Checks, tags, HealthConstants.HealthScore.healthy);
                }
            }

            var healthStatus = new HealthStatus(results.Where(h => !h.Check.Status.IsIgnored()));
            var overallHealthStatus = HealthConstants.HealthScore.healthy;

            if (healthStatus.Status == HealthCheckStatus.Unhealthy)
            {
                overallHealthStatus = HealthConstants.HealthScore.unhealthy;
            }
            else if (healthStatus.Status == HealthCheckStatus.Degraded)
            {
                overallHealthStatus = HealthConstants.HealthScore.degraded;
            }

            _metrics.Measure.Gauge.SetValue(HealthChecksMetricsRegistry.HealthGauge, overallHealthStatus);

            Logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}

