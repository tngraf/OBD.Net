#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="IObdDeviceConnection.cs" company="Tethys">
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

namespace Tethys.OBD
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for OBD device connections.
    /// </summary>
    public interface IObdDeviceConnection
    {
        /// <summary>
        /// Writes the specified string to the device.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>An async Task.</returns>
        Task WriteToDevice(string text);

        /// <summary>
        /// Reads available data from the device.
        /// </summary>
        /// <returns>A data string.</returns>
        Task<string> ReadFromDevice();

        /// <summary>
        /// Reads available data from the device until the end
        /// character is received.
        /// </summary>
        /// <param name="endSign">The end sign.</param>
        /// <returns>A data string.</returns>
        Task<string> ReadFromDevice(string endSign);
    } // IObdDeviceConnection
} // Tethys.OBD
