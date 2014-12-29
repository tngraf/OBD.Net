#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Bluetooth test application.
//
// ==========================================================================
// <copyright file="BluetoothManager.cs" company="Tethys">
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

namespace Tethys.OBD.BluetoothTest.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Windows.Networking.Proximity;
    using Windows.Networking.Sockets;

    /// <summary>
    /// Manages bluetooth devices and connections.
    /// </summary>
    public class BluetoothManager
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the output handler.
        /// </summary>
        public Action<string> OutputHandler { get; set; }

        /// <summary>
        /// Gets or sets the message box reporter.
        /// </summary>
        public Action<string> MessageBoxReporter { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Checks for bluetooth availability.
        /// </summary>
        /// <returns><c>true</c> if Bluetooth is enabled; otherwise <c>false</c>.
        /// </returns>
        public bool CheckForBluetooth()
        {
            try
            {
                // Connect to your paired OBD2 using BT + StreamSocket (over RFCOMM)
                PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = string.Empty;
            }
            catch
            {
                this.OutputText("Bluetooth not enabled!");
                this.ShowMessageBox("Bluetooth not enabled!");
                return false;
            } // catch

            this.OutputText("Bluetooth is enabled.");

            return true;
        } // CheckForBluetooth()

        /// <summary>
        /// Finds available bluetooth devices.
        /// </summary>
        /// <returns>A list of bluetooth devices.</returns>
        public async Task<List<PeerInformation>> FindBluetoothDevices()
        {
            this.CheckForBluetooth();

            var retDevices = new List<PeerInformation>();

            try
            {
                var devices = await PeerFinder.FindAllPeersAsync();

                if (devices == null)
                {
                    this.ShowMessageBox("No bluetooth devices are found, please pair OBD Interface");
                    return retDevices;
                } // if

                if (devices.Count == 0)
                {
                    this.ShowMessageBox("No bluetooth devices are paired, please pair OBD Interface");
                    return retDevices;
                } // if

                retDevices.AddRange(devices);
            }
            catch (Exception ex)
            {
                this.OutputText("Error finding devices: " + ex.Message);
            } // if

            return retDevices;
        } // FindBluetoothDevices()

        /// <summary>
        /// Connects to device.
        /// </summary>
        /// <param name="peerInfo">The peer information.</param>
        /// <returns>A stream socket.</returns>
        public async Task<StreamSocket> ConnectToDevice(PeerInformation peerInfo)
        {
            this.CheckForBluetooth();

            var socket = new StreamSocket();
            socket.Control.KeepAlive = true;
            socket.Control.NoDelay = true;

            try
            {
                await socket.ConnectAsync(peerInfo.HostName, "1");
            }
            catch (Exception ex)
            {
                this.OutputText("Error connecting to device: " + ex.Message);
                return null;
            } // catch

            return socket;
        } // ConnectToDevice()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Outputs the text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void OutputText(string text)
        {
            if (this.OutputHandler == null)
            {
                return;
            } // if

            this.OutputHandler(text);
        } // OutputText()

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="text">The text.</param>
        private void ShowMessageBox(string text)
        {
            if (this.MessageBoxReporter == null)
            {
                return;
            } // if

            this.MessageBoxReporter(text);
        } // ShowMessageBox()
        #endregion // PRIVATE METHODS
    } // BluetoothManager
}
