#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdManager.cs" company="Tethys">
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

namespace Tethys.OBD.ObdAnalyzer.Core
{
    /// <summary>
    /// Manages the connection to a OBD device.
    /// </summary>
    public class ObdManager : ObdManagerBase
    {
        #region PRIVATE PROPERTIES
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ObdManager" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public ObdManager(IObdDeviceConnection connection)
            : base(connection)
        {
        } // ObdManager()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        #endregion // PRIVATE METHODS
    } // ObdManager
} // Tethys.OBD.ObdAnalyzer.Core
