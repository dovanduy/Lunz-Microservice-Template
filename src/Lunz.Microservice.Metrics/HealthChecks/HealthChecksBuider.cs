using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.NoOp;
using App.Metrics.Health.Logging;
using System.Linq;

namespace Lunz.Microservice.Metrics
{
    public static class HealthChecksBuider
    {
        private static readonly HealthFormatterCollection _healthFormatterCollection = new HealthFormatterCollection();

        public static IHealthRoot HealthBuild(this IHealthRoot healthRoot, IMetrics metrics)
        {

            if (_healthFormatterCollection.Count == 0)
            {
                _healthFormatterCollection.Add(new HealthStatusTextOutputFormatter());
            }
            else
            {

                foreach (var item in healthRoot.OutputHealthFormatters)
                {
                    _healthFormatterCollection.Add(item);
                }
            }

            IRunHealthChecks healthCheckRunner;

            var health = new DefaultHealth(healthRoot.Checks);
            if (healthRoot.Options.Enabled && healthRoot.Checks.Any())
            {
                healthCheckRunner = new HealthChecksRunner(healthRoot.Checks, metrics);
            }
            else
            {
                healthCheckRunner = new NoOpHealthCheckRunner();
            }

            return new HealthRoot(
                health,
                healthRoot.Options,
                _healthFormatterCollection,
                healthRoot.DefaultOutputHealthFormatter,
                healthCheckRunner);
        }
    }
}

