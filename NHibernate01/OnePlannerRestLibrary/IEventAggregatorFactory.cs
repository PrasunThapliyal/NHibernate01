

namespace Ciena.BluePlanet.OnePlannerRestLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Practices.Prism.Events;

    /// <summary>
    /// 
    /// </summary>
    public interface IEventAggregatorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEventAggregator Create();
    }

    /// <summary>
    /// 
    /// </summary>
    public class EventAggregatorFactory : IEventAggregatorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEventAggregator Create()
        {
            return new EventAggregator();
        }
    }
}
