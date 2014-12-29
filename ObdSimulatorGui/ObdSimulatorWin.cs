#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdSimulatorWin.cs" company="Tethys">
// Copyright  2014 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... .Net Framework 4.5
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.ObdSimulator.UI
{
    using System.Globalization;
    using System.IO.Ports;

    using Tethys.Logging;
    using Tethys.OBD.ObdSimulator;

    /// <summary>
    /// OBS Simulator (for Windows) class.
    /// </summary>
    public class ObdSimulatorWin : ObdSimulator
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Logger for this class.
        /// </summary>
        private static readonly ILog Log 
            = LogManager.GetLogger(typeof(ObdSimulatorWin));
        
        /// <summary>
        /// Current command.
        /// </summary>
        private string command;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets the port.
        /// </summary>
        public SerialPort Port { get; private set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ObdSimulatorWin"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public ObdSimulatorWin(string port)
        {
            this.Port = new SerialPort(port);
            this.Port.DataReceived += this.PortOnDataReceived;
            this.Port.ErrorReceived += this.PortOnErrorReceived;
        } // ObdSimulatorWin()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Connect to the specified port.
        /// </summary>
        public void Connect()
        {
            this.Port.BaudRate = 115200;
            this.Port.DataBits = 8;
            this.Port.StopBits = StopBits.One;
            this.Port.Handshake = Handshake.None;

            this.Port.Open();

            Log.DebugFormat("Connected at {0}, {1} bps", this.Port.PortName,
                this.Port.BaudRate);
        } // Connect()

        /// <summary>
        /// Disconnects from the specified port.
        /// </summary>
        public void Disconnect()
        {
            this.Port.Close();
        } // Disconnect()
        #endregion // PUBLIC METHODS

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
                Log.ErrorFormat("Invalid command: '{0}'!", cmd);
            }
            else
            {
                Log.DebugFormat("Command '{0}' processed, answer = '{1}'",
                    cmd, retData);
                this.Port.Write(retData);
            } // if
        } // ProcessCommand()
        
        /// <summary>
        /// Handles the incoming data.
        /// </summary>
        private void HandleIncomingData()
        {
            var data = this.Port.ReadExisting();
            Log.DebugFormat("Received data '{0}'", data);

            foreach (var ch in data)
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
        } // HandleIncomingData()

        /// <summary>
        /// Ports the on data received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SerialDataReceivedEventArgs"/> 
        /// instance containing the event data.</param>
        private void PortOnDataReceived(object sender, 
            SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                this.HandleIncomingData();
            }
            else if (e.EventType == SerialData.Eof)
            {
                Log.Debug("Got EOF");
            } // if
        } // PortOnDataReceived()

        /// <summary>
        /// Ports the on error received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SerialErrorReceivedEventArgs"/>
        /// instance containing the event data.</param>
        private void PortOnErrorReceived(object sender, 
            SerialErrorReceivedEventArgs e)
        {
            Log.ErrorFormat(CultureInfo.CurrentCulture,
                "Error on serial port: {0}", e.EventType);
        } // PortOnErrorReceived()
        #endregion // PRIVATE METHODS
    } // ObdSimulatorWin
} // Tethys.OBD.ObdSimulator.UI
