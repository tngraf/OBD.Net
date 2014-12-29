#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// OBD via Bluetooth application for Windows Phone 8.0 and later.
//
// ==========================================================================
// <copyright file="LocalizedStrings.cs" company="Tethys">
// Copyright  2014 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Windows Phone 8
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.ObdAnalyzer.Views
{
    using Tethys.OBD.ObdAnalyzer.Resources;

    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        /// <summary>
        /// The localized resource.
        /// </summary>
        private static readonly AppResources LocalizedRes = new AppResources();

        /// <summary>
        /// Gets the localized resources.
        /// </summary>
        public AppResources LocalizedResources
        {
            get
            {
                return LocalizedRes;
            }
        }
    } // LocalizedStrings
} // Tethys.OBD.ObdAnalyzer.Views