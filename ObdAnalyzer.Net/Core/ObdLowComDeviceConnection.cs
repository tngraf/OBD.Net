#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdLowComDeviceConnection.cs" company="Tethys">
// Copyright  2014 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Portable Library
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.ObdAnalyzer.Net.Core
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Tethys.Logging;

    /// <summary>
    /// Connection to an OBD devices. 
    /// </summary>
    public class ObdLowComDeviceConnection : IObdDeviceConnection
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Logger for this class.
        /// </summary>
        private static readonly ILog Log
            = LogManager.GetLogger(typeof(ObdLowComDeviceConnection));

        /// <summary>
        /// Low level communication.
        /// </summary>
        private readonly ILowLevelCom lowcom;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ObdLowComDeviceConnection" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public ObdLowComDeviceConnection(ILowLevelCom connection)
        {
            this.lowcom = connection;
        } // ObdManagerBase()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
#pragma warning disable 1998 // suppress warning that the methods lack 'await'
        /// <summary>
        /// Writes the specified string to the device.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// An async Task.
        /// </returns>
        public async Task WriteToDevice(string text)
        {
            Log.DebugFormat("WriteToDevice = '{0}'", text);
            this.lowcom.Write(text);
        } // WriteToDevice()

        /// <summary>
        /// Reads available data from the device.
        /// </summary>
        /// <returns>A data string.</returns>
        public async Task<string> ReadFromDevice()
        {
            Thread.Sleep(100);
            var text = this.lowcom.ReadExisting();
            Log.DebugFormat("ReadFromDevice = '{0}'", text);
            return text;
        } // ReadFromDevice()

        /// <summary>
        /// Reads available data from the device until the end
        /// character is received.
        /// </summary>
        /// <param name="endSign">The end sign.</param>
        /// <returns>A data string.</returns>
        public async Task<string> ReadFromDevice(string endSign)
        {
            var start = DateTime.Now;
            var sb = new StringBuilder(10);
            var text = string.Empty;
            do
            {
                var newData = this.lowcom.ReadExisting();
                if (!string.IsNullOrEmpty(newData))
                {
                    sb.Append(newData);
                    text = sb.ToString();
                } // if

                var diff = DateTime.Now - start;
                if (diff.TotalMilliseconds > ObdManagerBase.ReadTimeout)
                {
                    break;
                } // if
            }
            while (!text.Contains(ObdBase.ElmPrompt));

            Log.DebugFormat("ReadFromDevice = '{0}'", text);
            return text;
        } // ReadFromDevice()
#pragma warning restore 1998
        #endregion // PUBLIC METHODS
    } // ObdLowComDeviceConnection
} // Tethys.OBD
