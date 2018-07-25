using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Lunz.Microservice.Metrics
{
    public static class MetricsBuilderExtensions
    {
        /// <summary>
        ///  Adds App Metrics MVC options for example adds a resource filter to provide app metrics with routes for tagging metrics.
        /// </summary>
        /// <param name="mvcBuilder">The <see cref="T:Microsoft.Extensions.DependencyInjection.IMvcBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.Extensions.DependencyInjection.IMvcBuilder" /> cannot be null
        /// </exception>
        public static IMvcBuilder AddMetricsExtensions(this IMvcBuilder mvcBuilder)
        {
            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }
            mvcBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<MvcOptions>, MetricsRouteOptionsConfig>());

            return mvcBuilder;
        }
    }
}
