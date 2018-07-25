using App.Metrics;
using App.Metrics.Gauge;

namespace Lunz.Microservice.Metrics
{
    public static class CpuRegistry
    {
        public static readonly string Context = "Application.Cpu";

        public static GaugeOptions CpuUsageGauge => new GaugeOptions
        {
            Context = Context,
            Name = "CPU_Usage",
            MeasurementUnit = Unit.Items
        };
    }
    public static class MemoryRegistry
    {
        public static readonly string Context = "Application.Memory";
        public static GaugeOptions MemoryUsageGauge => new GaugeOptions
        {
            Context = Context,
            Name = "Memory_Usage",
            MeasurementUnit = Unit.Items
        };
    }
}
