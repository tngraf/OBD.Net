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
// System ... .Net Framework 4.5
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.ObdAnalyzer.Net.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO.Ports;
    using System.Threading;
    using System.Threading.Tasks;

    using Tethys.Logging;

    /// <summary>
    /// The ObdManager handles the OBD connection.
    /// </summary>
    public class ObdManager : ObdManagerBase, IDisposable
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Logger for this class.
        /// </summary>
        private static readonly ILog Log
            = LogManager.GetLogger(typeof(ObdManager));

        /// <summary>
        /// Low level communication.
        /// </summary>
        private readonly ILowLevelCom lowcom;

        /// <summary>
        /// Disposed flag.
        /// </summary>
        private bool disposed;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets a value indicating whether this communication
        /// connection is valid.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (this.lowcom == null)
                {
                    return false;
                } // if

                return this.lowcom.IsOpen;
            }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ObdManager" /> class.
        /// </summary>
        /// <param name="lowLevelCommunication">The low level communication.</param>
        public ObdManager(ILowLevelCom lowLevelCommunication)
            : base(new ObdLowComDeviceConnection(lowLevelCommunication))
        {
            if (lowLevelCommunication == null)
            {
                throw new ArgumentNullException("lowLevelCommunication");
            } // if

            this.lowcom = lowLevelCommunication;
        } // ObdManager()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Open Connection to system.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful.</returns>
        public async Task<bool> OpenConnection()
        {
            if (!this.lowcom.IsOpen)
            {
                Log.DebugFormat(
                    CultureInfo.CurrentCulture,
                    "Opening connection: {0}, {1}, {2}, {3}, {4}, {5}",
                    this.lowcom.PortName, this.lowcom.BaudRate,
                    this.lowcom.DataBits, this.lowcom.Parity,
                    this.lowcom.StopBits, this.lowcom.Handshake);
                this.lowcom.Open();

                this.lowcom.DiscardInBuffer();
                this.lowcom.DiscardOutBuffer();
                this.lowcom.WriteTimeout = 4500;
            } // if

            Log.Debug("Connection opened, " + this.lowcom.ToString());

            await this.InitializeConnection();

            return this.lowcom.IsOpen;
        } // OpenConnection()

        /// <summary>
        /// Close connection to system.
        /// </summary>
        [SuppressMessage("Microsoft.Design",
          "CA1031:DoNotCatchGeneralExceptionTypes",
          Justification = "Ok for top level methods.")]
        public void CloseConnection()
        {
            if (this.lowcom.IsOpen)
            {
                this.lowcom.DiscardInBuffer();
                this.lowcom.DiscardOutBuffer();

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    // ignore all types of exception
                    Log.Warn("Problem closing connection: ", ex);
                } // catch

                this.lowcom.Close();

                Log.Debug("Connection closed.");

                this.lowcom.LowLevelComEvent -= this.OnLowLevelComEvent;
            } // if
        } // CloseConnection()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS

        #endregion // PRIVATE METHODS

        //// ---------------------------------------------------------------------

        #region (SERIAL) PORT EVENTS
        /// <summary>
        /// Event handler for low level communication events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LowLevelComEventArgs"/> instance
        /// containing the event data.</param>
        private void OnLowLevelComEvent(object sender, LowLevelComEventArgs e)
        {
            switch (e.EventType)
            {
                case LowLevelComEventType.DataReceived:
                    // not handled here
                    break;
                case LowLevelComEventType.ErrorReceived:
                    var errorReceivedEventArgs = e.EventData as SerialErrorReceivedEventArgs;
                    if (errorReceivedEventArgs != null)
                    {
                        OnPortErrorReceived(this,
                          (errorReceivedEventArgs).EventType);
                    } // if
                    break;
                case LowLevelComEventType.SerialPinChange:
                    var serialPinChangedEventArgs = e.EventData as SerialPinChangedEventArgs;
                    if (serialPinChangedEventArgs != null)
                    {
                        this.OnPortPinChanged(this,
                          (serialPinChangedEventArgs).EventType);
                    } // if
                    break;
                default:
                    Log.Warn("Unhandled low level communication event!");
                    break;
            } // switch
        } // lowcom_LowLevelComEvent()

        /// <summary>
        /// Called when a port pin has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="pinChange">The pin change.</param>
        // ReSharper disable UnusedParameter.Local
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
            MessageId = "sender", Justification = "Required parameter for event handler")]
        private void OnPortPinChanged(object sender, SerialPinChange pinChange)
        // ReSharper restore UnusedParameter.Local
        {
            switch (pinChange)
            {
                case SerialPinChange.Break:
                    Log.Info("break received! => might be ZAE reset");
                    break;
                case SerialPinChange.CDChanged:
                    Log.Info("CDChanged received!");
                    break;
                case SerialPinChange.CtsChanged:
                    Log.Info("CtsChanged received!");
                    break;
                case SerialPinChange.DsrChanged:
                    // ignore
                    // log.Info("DsrChanged received!");
                    break;
                case SerialPinChange.Ring:
#if DEBUG
                    // Ring Indicator (RI), pin 9, is not connected for the Transliner
                    // Ringbus cable
                    // ==> no need to track or to display
                    Log.Info("Ring received!");
#endif
                    break;
            } // switch
        } // OnPortPinChanged()

        /// <summary>
        /// Called when a port error has been received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="error">The error.</param>
        // ReSharper disable UnusedParameter.Local
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
            MessageId = "sender", Justification = "Required parameter for event handler")]
        private static void OnPortErrorReceived(object sender, SerialError error)
        // ReSharper restore UnusedParameter.Local
        {
            switch (error)
            {
                case SerialError.Frame:
                    Log.Warn("Framing error received!");
                    break;
                case SerialError.Overrun:
                    Log.Warn("Overrun error received!");
                    break;
                case SerialError.RXOver:
                    Log.Warn("RXOver error received!");
                    break;
                case SerialError.RXParity:
                    Log.Warn("RXParity error received!");
                    break;
                case SerialError.TXFull:
                    Log.Warn("TXFull error received!");
                    break;
            } // switch
        } // OnPortErrorReceived()
        #endregion // SERIAL PORT EVENTS

        //// ---------------------------------------------------------------------

        #region IDISPOSABLE MEMBERS
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        } // Dispose()

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed
        /// and unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.lowcom.Dispose();
                } // if

                // Flag: disposing has been done.
                this.disposed = true;
            } // if
        } // Dispose()
        #endregion
    } // ObdManager
} // Tethys.OBD.ObdAnalyzer.Net.Core
