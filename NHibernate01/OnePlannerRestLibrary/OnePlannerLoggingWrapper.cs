//-----------------------------------------------------------------------------
// <copyright file="OnePlannerLoggingWrapper.cs" company="Ciena Corporation">
//     Copyright (c) Ciena Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

namespace Ciena.BluePlanet.OnePlannerRestLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LABWrapper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using OnePlanner.CommonCS.Logging;

    /// <summary>
    /// A wrapper to redirect OnePlanner logging to the ASP.NET logging.
    /// </summary>
    /// <seealso cref="OnePlanner.CommonCS.Logging.ILog" />
    /// <seealso cref="LABWrapper.ICOMLog" />
    public class OnePlannerLoggingWrapper : ILog, ICOMLog
    {
        private ILogger _logger;
        private bool _powerUserMode;

#pragma warning disable 0067 // disable warning for unused variable. It is part of the ICOMLog interface so must be defined

        /// <summary>
        /// Occurs when OutputBoxEvent is fired.
        /// </summary>
        public event OutputBoxEventHandler OutputBoxEvent;

#pragma warning restore 0067

        /// <summary>
        /// Initializes a new instance of the <see cref="OnePlannerLoggingWrapper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OnePlannerLoggingWrapper(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        /// <param name="CommonCategory">The common category.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetCategoryName(Category CommonCategory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logCategory">The log category.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetCategoryName<T>(T logCategory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified severity.
        /// </summary>
        /// <param name="Severity">The severity.</param>
        /// <param name="Message">The message.</param>
        /// <param name="ParamArray">The parameter array.</param>
        public void Write(Severity Severity, string Message, string ParamArray)
        {
            Write(Severity, Message);
        }

        /// <summary>
        /// Writes the specified severity.
        /// </summary>
        /// <param name="Severity">The severity.</param>
        /// <param name="Message">The message.</param>
        /// <param name="ParamArray">The parameter array.</param>
        public void Write(Severity Severity, string Message, string[] ParamArray)
        {
            Write(Severity, Message);
        }

        private void Write(Severity Severity, string Message)
        {
            switch (Severity)
            {
                case Severity._Verbose:
                    _logger.LogTrace(Message);
                    break;
                case Severity._Information:
                    _logger.LogInformation(Message);
                    break;
                case Severity._Warning:
                    _logger.LogWarning(Message);
                    break;
                case Severity._Error:
                    _logger.LogError(Message);
                    break;
                case Severity._Critical:
                    _logger.LogCritical(Message);
                    break;
                default:
                    _logger.LogDebug(Message);
                    break;
            }
        }

        bool ICOMLog.PowerUserMode
        {
            get
            {
                return _powerUserMode;
            }

            set
            {
                _powerUserMode = value;
            }
        }

        /// <summary>
        /// AllowLogToWrite
        /// </summary>
        public bool AllowLogToWrite { set { } }

        Tracer ICOMLog.CreateTracer(COMLogCategories uclsLogCategories)
        {
            throw new NotImplementedException();
        }

        void ICOMLog.DebugWrite(string message, COMLog.UELogSeverity severity, COMLogCategories uclsLogCategories, bool blnOutputBox, Exception ex)
        {
            switch (severity)
            {
                case COMLog.UELogSeverity._Critical:
                    _logger.LogCritical(message);
                    break;
                case COMLog.UELogSeverity._Error:
                    _logger.LogError(message);
                    break;
                case COMLog.UELogSeverity._Information:
                    _logger.LogInformation(message);
                    break;
                case COMLog.UELogSeverity._Verbose:
                    _logger.LogTrace(message);
                    break;
                default:
                    _logger.LogDebug(message);
                    break;
            }
        }

        bool ICOMLog.ShouldLog(COMLog.UELogSeverity severity, COMLogCategories uclsLogCategories)
        {
            if (uclsLogCategories.IsSelected(COMLogCategories.UECategory.Performance))
            {
                return false;
            }
            return true;
        }
        void ICOMLog.Write(string message, COMLog.UELogSeverity severity, COMLogCategories uclsLogCategories, bool blnOutputBox, Exception ex, bool blnWarning)
        {
            ((ICOMLog)this).DebugWrite(message, severity, uclsLogCategories, blnOutputBox, ex);
        }

    }
}
