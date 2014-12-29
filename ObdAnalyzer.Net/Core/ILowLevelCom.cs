#region Header
// ---------------------------------------------------------------------------
// S e r i a l P o r t S u p p o r t
// ===========================================================================
//
// This library contains common code for Tethys projects.
//
// ===========================================================================
// <copyright file="ILowLevelCom.cs" company="Tethys">
// Copyright  2010-2014 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Microsoft .Net Framework 4
// Tools .... Microsoft Visual Studio 2010
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.ObdAnalyzer.Net.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Ports;
    using System.Text;

    /// <summary>
    /// Delegate for low level async callbacks.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="ex">Exception or null</param>
    public delegate void LowLevelAsyncCallback(object sender, Exception ex);

    /// <summary>
    /// Delegate for low level async callbacks.
    /// </summary>
    /// <param name="bytesRead">The number of bytes received.</param>
    /// <param name="ex">Exception or null</param>
    public delegate void LowLevelAsyncReadCallback(int bytesRead, Exception ex);

    /// <summary>
    /// ILowLevelCom is the interface for low level communication methods.
    /// </summary>
    public interface ILowLevelCom : IDisposable
    {
        /// <summary>
        /// Represents the method that handles low level communication events.
        /// </summary>
        event EventHandler<LowLevelComEventArgs> LowLevelComEvent;

        #region PROPERTIES
        /// <summary>
        /// Gets a value indicating what kind of communication device this is.
        /// </summary>
        string DeviceType { get; }

        /// <summary>
        /// Gets a value indicating whether the data connection is currently open.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Gets or sets the method of encoding and decoding text to and from 
        /// binary data.
        /// Defaults to System.IO.Encoding.ASCIIEncoding. Any arbitrary member
        /// of the Encoding class can be used.  Used to transform data for
        /// ReadLine(), ReadAvailable(), Read() and Read(char[]…)
        /// methods on the receiving end of input, and Write(string),
        /// Write(char[] …) for writing. 
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the timeout for all read operations.
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the timeout value for SerialPort Write methods.  May take the value
        /// SerialPort.InfiniteTimeout, or any positive integer.
        /// Defaults to SerialPort.InfiniteTimeout, expressed in milliseconds.
        /// If an entire Write(…) operation is not completed within the
        /// specified timeout, each throws a TimeoutException.  Throws an
        /// ArgumentOutOfRange exception if set to an integer less than zero. 
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        int WriteTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable Data Transmit
        /// Ready line (DTR) during serial
        /// communication. Defaults to false. Typically enabled during XOn/XOff
        /// and RTS/CTS handshaking and modem communication.  When DtrEnable
        /// is set to True, the Data Terminal Ready line is set to high (on)
        /// when the port is opened, and low (off) when the port is closed.
        /// When DtrEnable is set to False, the Data Terminal Ready always
        /// remains low.  Toggling the DtrEnable line causes modem hang up,
        /// additionally.
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        bool DtrEnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send 
        /// (RTS) signal is enabled during serial communication.
        /// </summary>
        bool RtsEnable { get; set; }

        /// <summary>
        /// Gets or sets a string representing name of communications resource. 
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        string PortName { get; set; }

        /// <summary>
        /// Gets or sets the positive value of baud rate of instance’s serial communication.
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        int BaudRate { get; set; }

        /// <summary>
        /// Gets a value indicating whether the device detects whether 
        /// a carrier signal exists on the
        /// serial port, or equivalently, state of the device’s CD pin. 
        /// Carrier Detect (CD) is also known as Receive-Line Signal Detect (RLSD).
        /// </summary>
        /// <remarks>
        /// LowLevelSerialPort: Read-only, and especially important to determine if
        /// a connected computer has hung up at any time.  Changes in CDHolding
        /// are also reflected in the PinChangedEvent thrown with the
        /// SerialEvents.PinChanged code.
        /// </remarks>
        bool CDHolding { get; }

        /// <summary>
        /// Gets a value indicating whether data can be sent, determined by 
        /// querying the CTS line of the
        /// serial port.  
        /// </summary>
        /// <remarks>
        /// LowLevelSerialPort: Read-only, and used in RTS/CTS hardware handshaking. 
        /// Typically, an attached modem sets the line to indicate that transmission
        /// can proceed. 
        /// </remarks>
        bool CtsHolding { get; }

        /// <summary>
        /// Gets a value indicating whether SerialPort transmission is currently
        /// in a break state.
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        bool InBreak { get; }

        /// <summary>
        /// Gets or sets the number of data bits per serial byte.
        /// Defaults to 8 but must be in the range of [5,8].
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the method of setting/reading parity bit on each byte 
        /// serial I/O.
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the number of stop bits per serial byte.
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets the protocol for flow control. 
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        Handshake Handshake { get; set; }

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine methods.
        /// </summary>
        /// <remarks>Used by LowLevelSerialPort</remarks>
        string NewLine { get; set; }

        /// <summary>
        /// Gets the number of byte of data in the receive buffer.
        /// </summary>
        int BytesToRead { get; }

        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        int BytesToWrite { get; }

        /// <summary>
        /// Gets or sets the size of the serial port input buffer.
        /// </summary>
        int ReadBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the serial port output buffer. 
        /// </summary>
        int WriteBufferSize { get; set; }
        #endregion // PROPERTIES

        /// <summary>
        /// Opens (synchronously) a connection.
        /// </summary>
        void Open();

        /// <summary>
        /// Opens (asynchronously) a connection.
        /// </summary>
        /// <param name="userCallback">Callback method to notify application</param>
        void Open(LowLevelAsyncCallback userCallback);

        /// <summary>
        /// Closes (synchronously) the current connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Returns -1 on timeout, or the first available full char if found before.
        /// </summary>
        /// <returns>
        /// -1 on timeout, or the first available full char if found before.
        /// </returns>
        int Read();

        /// <summary>
        /// Reads (synchronously) from the current TCP connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <returns>The number of bytes received.</returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            MessageId = "0#", Justification = "See Microsoft SerialPort")]
        int Read(ref byte[] buffer, int offset, int len);

        /// <summary>
        /// Reads (asynchronously) from the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            MessageId = "0#", Justification = "See Microsoft SerialPort")]
        void Read(ref byte[] buffer, int offset, int len,
          LowLevelAsyncReadCallback userCallback);

        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer.</returns>
        string ReadExisting();

        /// <summary>
        /// Writes (synchronously) to the current connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        void Write(string data);

        /// <summary>
        /// Writes (synchronously) to the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that containing the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        void Write(byte[] buffer, int offset, int len);

        /// <summary>
        /// Writes (asynchronously) to the current connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        void Write(string data, LowLevelAsyncCallback userCallback);

        /// <summary>
        /// Writes (asynchronously) to the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that containing the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        void Write(byte[] buffer, int offset, int len,
          LowLevelAsyncCallback userCallback);

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        void WriteLine(string text);

        /// <summary>
        /// Discards data from the low level communication receive buffer.
        /// </summary>
        void DiscardInBuffer();

        /// <summary>
        /// Discards data from the low level communication send buffer.
        /// </summary>
        void DiscardOutBuffer();

        /// <summary>
        /// Returns a string representing this object.
        /// </summary>
        /// <returns>A string representing this object</returns>
        string ToString();
    } // ILowLevelCom
} // Tethys.OBD.ObdAnalyzer.Net.Core

// ======================
// End of ILowlevelCom.cs
// ======================
