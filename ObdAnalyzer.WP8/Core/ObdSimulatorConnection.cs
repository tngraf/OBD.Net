#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdSimulatorConnection.cs" company="Tethys">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Tethys.OBD.ObdSimulator;

    /// <summary>
    /// An <see cref="IObdDeviceConnection"/> implementation that uses the
    /// <see cref="Tethys.OBD.ObdSimulator.ObdSimulator"/>.
    /// </summary>
    public class ObdSimulatorConnection : ObdSimulator, IObdDeviceConnection
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Output queue.
        /// </summary>
        private readonly Queue<string> queue;

        /// <summary>
        /// Current command.
        /// </summary>
        private string command;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ObdSimulatorConnection"/> class.
        /// </summary>
        public ObdSimulatorConnection()
        {
            this.queue = new Queue<string>(20);
        } // ObdSimulatorConnection()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Writes the specified string to the device.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// An async Task.
        /// </returns>
#pragma warning disable 1998
        public async Task WriteToDevice(string text)
#pragma warning restore 1998
        {
            foreach (var ch in text)
            {
                if ((ch == '\r') || (ch == '\n'))
                {
                    this.ProcessCommand();
                }
                else
                {
                    this.command += ch;
                } // if
            } // foreach
        } // WriteToDevice()

        /// <summary>
        /// Reads available data from the device.
        /// </summary>
        /// <returns>A data string.</returns>
#pragma warning disable 1998
        public async Task<string> ReadFromDevice()
#pragma warning restore 1998
        {
            Thread.Sleep(100);
            var text = this.ReadExisting();
            return text;
        } // ReadFromDevice()

        /// <summary>
        /// Reads available data from the device until the end
        /// character is received.
        /// </summary>
        /// <param name="endSign">The end sign.</param>
        /// <returns>A data string.</returns>
#pragma warning disable 1998
        public async Task<string> ReadFromDevice(string endSign)
#pragma warning restore 1998
        {
            Thread.Sleep(100);
            var text = this.ReadExisting();
            return text;
        } // ReadFromDevice()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        #endregion // PROTECTED METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Processes the command.
        /// </summary>
        private void ProcessCommand()
        {
            if (string.IsNullOrEmpty(this.command))
            {
                return;
            } // if

            var cmd = this.command;
            var retData = base.ProcessCommand(cmd);
            this.command = string.Empty;

            if (retData == ObdSimulator.InvalidCommand)
            {
                if (this.ShowLogging)
                {
                    Debug.WriteLine("Invalid command: '{0}'!", cmd);
                }
            }
            else
            {
                if (this.ShowLogging)
                {
                    Debug.WriteLine("Command '{0}' processed, answer = '{1}'",
                        cmd, retData);
                } // if
                this.queue.Enqueue(retData);
            } // if
        } // ProcessCommand()

        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in the 
        /// input buffer.
        /// </summary>
        /// <returns>
        /// The contents of the input buffer.
        /// </returns>
        private string ReadExisting()
        {
            var sb = new StringBuilder(20 * (this.queue.Count + 1));
            while (this.queue.Count > 0)
            {
                sb.Append(this.queue.Dequeue());
            } // while

            return sb.ToString();
        } // ReadExisting()
        #endregion // PRIVATE METHODS
    } // ObdSimulatorConnection
} // Tethys.OBD.ObdAnalyzer.Core
