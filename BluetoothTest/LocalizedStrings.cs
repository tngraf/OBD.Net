#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Bluetooth test application.
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
// System ... Microsoft Windows Phone 8
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.BluetoothTest
{
    using Tethys.OBD.BluetoothTest.Resources;

    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        /// <summary>
        /// The localized resources.
        /// </summary>
        private static AppResources localizedResources = new AppResources();

        /// <summary>
        /// Gets the localized resources.
        /// </summary>
        public AppResources LocalizedResources
        {
            get
            {
                return localizedResources;
            }
        }
    }
}