

namespace Ciena.BluePlanet.OnePlannerRestLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Practices.Prism.Events;
    using OnePlanner.CommonCS.Commands;

    /// <summary>
    /// 
    /// </summary>
    public interface IUndoManagerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IUndoManager Create(IEventAggregator eventAggregator);
    }

    /// <summary>
    /// 
    /// </summary>
    public class UndoManagerFactory : IUndoManagerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IUndoManager Create(IEventAggregator eventAggregator)
        {
            var undoManager = new UndoManager(eventAggregator);
            undoManager.AutoFlushCommandsStack = true;
            return undoManager;
        }
    }
}
