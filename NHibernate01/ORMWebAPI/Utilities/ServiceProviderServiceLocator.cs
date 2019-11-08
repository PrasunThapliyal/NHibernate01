//-----------------------------------------------------------------------------
// <copyright file="ServiceProviderServiceLocator.cs" company="Ciena Corporation">
//     Copyright (c) Ciena Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------
namespace Ciena.BluePlanet.TopologyPlanningService.Utilities
{
    using CommonServiceLocator;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Service Locator for Service Provider
    /// </summary>
    public class ServiceProviderServiceLocator : ServiceLocatorImplBase
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Setup ServiceLocator using serviceProvider
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ServiceProviderServiceLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Resolves dependency using serviceProvider
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return _serviceProvider.GetService(serviceType);
        }

        /// <summary>
        /// Resolves dependency using serviceProvider
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }
    }
}
