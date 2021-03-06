﻿using App.Metrics.Health;
using App.Metrics.Health.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace Lunz.Microservice.Metrics
{
    public static class HealthChecksLogger
    {
        internal static void HealthCheckGetStatusExecuted(
            this ILog logger,
            HealthStatus healthStatus,
            long startTimestamp)
        {
            if (!logger.IsTraceEnabled())
            {
                return;
            }

            if (startTimestamp == 0)
            {
                return;
            }

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            if (healthStatus.Results.Any())
            {
                if (healthStatus.Status.IsHealthy())
                {
                    logger.Info(
                        "Executed HealthStatus, in {ElapsedMilliseconds}ms, IsHealthy: True. {ChecksPassed} health check results passed.",
                        elapsed.TotalMilliseconds,
                        healthStatus.Results.Count());
                    return;
                }

                var checksFailed = healthStatus.Results.Count(x => x.Check.Status.IsUnhealthy());
                var checksDegraded = healthStatus.Results.Count(x => x.Check.Status.IsDegraded());
                var checksPassed = healthStatus.Results.Count(x => x.Check.Status.IsHealthy());
                var failedChecks = healthStatus.Results.Where(h => h.Check.Status.IsUnhealthy()).Select(h => h.Name);
                var degradedChecks = healthStatus.Results.Where(h => h.Check.Status.IsDegraded()).Select(h => h.Name);

                logger.Info(
                    "Executed HealthStatus, in {ElapsedMilliseconds}ms, IsHealthy: False. {ChecksPassed} health check results passed. {ChecksFailed} health check results failed. Failed Checks: {FailedChecks}. {ChecksDegraded} health check results degredated. Degraded Checks: {DegredatedChecks}",
                    elapsed.TotalMilliseconds,
                    checksFailed,
                    checksDegraded,
                    checksPassed,
                    degradedChecks,
                    failedChecks);

                return;
            }

            logger.Info("Executed HealthStatus, 0 health check results.");
        }

        internal static void HealthCheckGetStatusExecuting(this ILog logger)
        {
            logger.Trace("Executing HealthCheck Get Status");
        }

        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
    }
}
