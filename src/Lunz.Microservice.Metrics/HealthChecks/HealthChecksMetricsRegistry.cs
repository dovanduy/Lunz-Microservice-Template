using App.Metrics;
using App.Metrics.Gauge;

namespace Lunz.Microservice.Metrics
{
    public class HealthChecksMetricsRegistry
    {
        public static readonly string Context = "Application.Health";

        public static GaugeOptions Checks => new GaugeOptions
        {
            Context = Context,
            Name = "Results",
            MeasurementUnit = Unit.Items
        };

        public static GaugeOptions HealthGauge => new GaugeOptions
        {
            Context = Context,
            Name = "Score",
            MeasurementUnit = Unit.Custom("Health Score")
        };

        public static class TagKeys
        {
            public const string HealthCheckName = "health_check_name";
            public const string HealthCheckStatus = "health_check_status";
        }
    }
}
