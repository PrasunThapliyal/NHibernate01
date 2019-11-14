//-----------------------------------------------------------------------------
// <copyright file="RptVersionUtility.cs" company="Ciena Corporation">
//     Copyright (c) Ciena Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace Ciena.BluePlanet.OnePlannerRestLibrary.Utilities
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// RPT Version Utility
    /// </summary>
    public class RptVersionUtility : IRptVersionUtility
    {
        private readonly ILogger<RptVersionUtility> _logger;

        /// <summary>
        /// Initializes a new instance of the RPT Version Utility
        /// </summary>
        /// <param name="logger"></param>
        public RptVersionUtility(ILogger<RptVersionUtility> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Returns a string containing the executing RPT app version
        /// </summary>
        /// <returns></returns>
        public string RptAppVersion()
        {
            //The BP_APP_VERSION environment variable returns mcp version information in the host environment
            //It will return null in the development environment
            var appVersionInHostEnv = Environment.GetEnvironmentVariable("BP_APP_VERSION");
            _logger.LogInformation("BP_APP_VERSION env variable received as: " + appVersionInHostEnv);
            string fileSourceWithVersion = "MCP";
            if (!string.IsNullOrEmpty(appVersionInHostEnv))
            {
                fileSourceWithVersion = string.Concat(fileSourceWithVersion, "(", appVersionInHostEnv, ")");
            }
            return fileSourceWithVersion;
        }
    }
}
