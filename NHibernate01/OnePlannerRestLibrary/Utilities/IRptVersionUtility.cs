//-----------------------------------------------------------------------------
// <copyright file="IRptVersionUtility.cs" company="Ciena Corporation">
//     Copyright (c) Ciena Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace Ciena.BluePlanet.OnePlannerRestLibrary.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the RPT Version Utility
    /// </summary>
    public interface IRptVersionUtility
    {
        /// <summary>
        /// Returns a string containing the executing RPT app version
        /// </summary>
        /// <returns></returns>
        string RptAppVersion();
    }
}
