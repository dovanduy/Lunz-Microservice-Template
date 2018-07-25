using App.Metrics.Health;
using System.IO;
using System.Threading.Tasks;

namespace Lunz.Microservice.Metrics
{
    /// <summary>
    /// 健康检查配置项
    /// </summary>
    public class HealthChecksConfig
    {
        /// <summary>
        /// 剩余硬盘空间阈值
        /// </summary>
        public int FreeDiskSize { get; set; }
        /// <summary>
        /// 剩余专属内存阈值
        /// </summary>
        public int PrivateMemorySize { get; set; }
        /// <summary>
        /// 剩余虚拟内存阈值
        /// </summary>
        public int VirtualMemorySize { get; set; }
        /// <summary>
        /// 剩余占用内存阈值
        /// </summary>
        public int PhysicalMemorySize { get; set; }
    }

    /// <summary>
    /// 健康检查初始化
    /// </summary>
    public class HealthRootBuilder
    {
        private readonly HealthChecksConfig _settings;
        public HealthRootBuilder(HealthChecksConfig settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// 剩余硬盘阈值(G)
        /// </summary>
        private int FreeDiskSize
        {
            get
            {
                return _settings.FreeDiskSize;
            }
        }

        /// <summary>
        /// 专属内存阈值(G)
        /// </summary>
        private int PrivateMemorySize
        {
            get
            {
                return _settings.PrivateMemorySize;
            }
        }

        /// <summary>
        /// 虚拟内存阈值(G)
        /// </summary>
        private int VirtualMemorySize
        {
            get
            {
                return _settings.VirtualMemorySize;
            }
        }

        /// <summary>
        /// 占用内存阈值(G)
        /// </summary>
        private int PhysicalMemorySize
        {
            get
            {
                return _settings.PhysicalMemorySize;
            }
        }

        /// <summary>
        /// 获取系统硬盘可用空间
        /// </summary>
        /// <returns></returns>
        public static double GetAllFreeDisk()
        {
            try
            {
                var disks = Directory.GetLogicalDrives();
                var totalFreeSpace = 0L;
                if (disks == null || disks.Length == 0)
                    return 0D;
                foreach (var disk in disks)
                {
                    var diskInfo = new DriveInfo(disk);
                    totalFreeSpace = totalFreeSpace + diskInfo.TotalFreeSpace;
                }
                return totalFreeSpace * 1.0 / (1024 * 1024 * 1024);
            }
            catch
            {
                return 0D;
            }

        }

        /// <summary>
        /// 健康检查healthbuilder配置
        /// </summary>
        public IHealthRoot HealthBuilder
        {
            get
            {
                var healthBuilder = AppMetricsHealth.CreateDefaultBuilder()
                   .HealthChecks.AddProcessPrivateMemorySizeCheck(string.Format("专用内存占用量是否超过阀值({0}G)", PrivateMemorySize), (PrivateMemorySize * 1024L * 1024L) * 1024L)
                   .HealthChecks.AddProcessVirtualMemorySizeCheck(string.Format("虚拟内存占用量是否超过阀值({0}G)", VirtualMemorySize), (VirtualMemorySize * 1024L * 1024L) * 1024L)
                   .HealthChecks.AddProcessPhysicalMemoryCheck(string.Format("占用内存是否超过阀值({0}G)", PhysicalMemorySize), (PhysicalMemorySize * 1024L * 1024L) * 1024L)
                   .HealthChecks.AddCheck
                   (string.Format("硬盘剩余空间是否超过阈值({0}G)", FreeDiskSize),
                   () =>
                     {
                         var freeDiskSpace = GetAllFreeDisk();
                         return new ValueTask<HealthCheckResult>(
                             freeDiskSpace <= FreeDiskSize
                                 ? HealthCheckResult.Unhealthy("硬盘可用空间不足: {0}", freeDiskSpace)
                                 : HealthCheckResult.Healthy("硬盘可用空间充足: {0}", freeDiskSpace));
                     })
                 .Build();

                return healthBuilder;
            }
        }
    }
}
