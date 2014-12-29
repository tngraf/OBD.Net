#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdStreamSocketDeviceConnection.cs" company="Tethys">
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
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    /// <summary>
    /// Connection to an OBD devices. 
    /// </summary>
    public class ObdStreamSocketDeviceConnection : IObdDeviceConnection
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Gets or sets the writer.
        /// </summary>
        private readonly DataWriter writer;

        /// <summary>
        /// Gets or sets the reader.
        /// </summary>
        private readonly DataReader reader;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------
  
        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ObdStreamSocketDeviceConnection" /> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        public ObdStreamSocketDeviceConnection(StreamSocket socket)
        {
            this.writer = new DataWriter(socket.OutputStream);
            this.reader = new DataReader(socket.InputStream);
            this.reader.InputStreamOptions = InputStreamOptions.Partial;
        } // ObdStreamSocketDeviceConnection()
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
        public async Task WriteToDevice(string text)
        {
            try
            {
                this.writer.WriteString(text);
                await this.writer.StoreAsync();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
                // IGNORE
                Debug.WriteLine("Error in WriteToDevice: " + ex.Message);
            } // catch 
        } // WriteToDevice()

        /// <summary>
        /// Reads available data from the device.
        /// </summary>
        /// <returns>A data string.</returns>
        public async Task<string> ReadFromDevice()
        {
            var text = string.Empty;

            try
            {
                var retries = 3;
                uint available;
                do
                {
                    Thread.Sleep(20);
                    available = await this.reader.LoadAsync(1000);
                }
                while ((available == 0) && (retries-- > 0));
                
                if (available == 0)
                {
                    return string.Empty;
                } // if

                if (this.reader.UnconsumedBufferLength > 0)
                {
                    text = this.reader.ReadString(this.reader.UnconsumedBufferLength);
                } // if
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // IGNORE
            } // catch 

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
            var text = string.Empty;

            try
            {
                var start = DateTime.Now;

                var available = await this.reader.LoadAsync(100);
                if (available == 0)
                {
                    return string.Empty;
                } // if

                var sb = new StringBuilder(100);
                do
                {
                    if (this.reader.UnconsumedBufferLength > 0)
                    {
                        sb.Append(this.reader.ReadString(this.reader.UnconsumedBufferLength));
                        text = sb.ToString();
                    }
                    else
                    {
                        available = await this.reader.LoadAsync(10);
                        if (available == 0)
                        {
                            // no more data available
                            return string.Empty;
                        } // if
                    } // if

                    var diff = DateTime.Now - start;
                    if (diff.TotalMilliseconds > ObdManagerBase.ReadTimeout)
                    {
                        break;
                    } // if
                }
                while (!text.Contains(ObdBase.ElmPrompt));
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // IGNORE
            } // catch 

            return text;
        } // ReadFromDevice()
        #endregion // PUBLIC METHODS
    } // ObdStreamSocketDeviceConnection
} // Tethys.OBD
