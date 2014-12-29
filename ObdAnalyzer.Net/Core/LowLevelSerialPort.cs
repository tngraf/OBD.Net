#region Header
// ---------------------------------------------------------------------------
// S e r i a l P o r t S u p p o r t
// ===========================================================================
//
// This library contains common code for Tethys projects.
//
// ===========================================================================
// <copyright file="LowLevelSerialPort.cs" company="Tethys">
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
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.ObdAnalyzer.Net.Core
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO.Ports;
    using System.Text;

    /// <summary>
    /// LowLevelSerialPort implements the low level communication methods
    /// to access the serial port.
    /// </summary>
    public class LowLevelSerialPort : ILowLevelCom
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Serial port.
        /// </summary>
        private readonly SerialPort port;

        /// <summary>
        /// Disposed flag.
        /// </summary>
        private bool disposed;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region ILOWLEVELCOM PROPERTIES
        /// <summary>
        /// Represents the method that handles low level communication events.
        /// </summary>
        public event EventHandler<LowLevelComEventArgs> LowLevelComEvent;

        /// <summary>
        /// Gets a value indicating what kind of communication device this is.
        /// </summary>
        public string DeviceType
        {
            get { return "SerialPort"; }
        }

        /// <summary>
        /// Gets a value indicating whether the data connection is currently open.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.IsOpen;
            }
        }

        /// <summary>
        /// Gets or sets the method of encoding and decoding text to and from
        /// binary data.
        /// Defaults to System.IO.Encoding.ASCIIEncoding. Any arbitrary member
        /// of the Encoding class can be used.  Used to transform data for
        /// ReadLine(), ReadAvailable(), Read() and Read(char[]…)
        /// methods on the receiving end of input, and Write(string),
        /// Write(char[] …) for writing.
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.Encoding;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.Encoding = value;
            }
        } // Encoding

        /// <summary>
        /// Gets or sets the timeout for all read operations.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public int ReadTimeout
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.ReadTimeout;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.ReadTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the timeout value for SerialPort Write methods.  May take the value
        /// SerialPort.InfiniteTimeout, or any positive integer.
        /// Defaults to SerialPort.InfiniteTimeout, expressed in milliseconds.
        /// If an entire Write(…) operation is not completed within the
        /// specified timeout, each throws a TimeoutException.  Throws an
        /// ArgumentOutOfRange exception if set to an integer less than zero.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public int WriteTimeout
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.WriteTimeout;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.WriteTimeout = value;
            }
        }

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
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public bool DtrEnable
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.DtrEnable;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.DtrEnable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send 
        /// (RTS) signal is enabled during serial communication.
        /// </summary>
        public bool RtsEnable
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.RtsEnable;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.RtsEnable = value;
            }
        }

        /// <summary>
        /// Gets or sets a string representing name of communications resource.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public string PortName
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.PortName;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.PortName = value;
            }
        }

        /// <summary>
        /// Gets or sets the positive value of baud rate of instance’s serial communication.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public int BaudRate
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.BaudRate;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.BaudRate = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the device detects whether
        /// a carrier signal exists on the
        /// serial port, or equivalently, state of the device’s CD pin.
        /// Carrier Detect (CD) is also known as Receive-Line Signal Detect (RLSD).
        /// </summary>
        /// <remarks>
        /// Read-only, and especially important to determine if a connected computer
        /// has hung up at any time.  Changes in CDHolding are also reflected in
        /// the PinChangedEvent thrown with the SerialEvents.PinChanged code.
        /// </remarks>
        public bool CDHolding
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.CDHolding;
            }
        }

        /// <summary>
        /// Gets a value indicating whether data can be sent, determined by
        /// querying the CTS line of the
        /// serial port.
        /// </summary>
        /// <remarks>
        /// Read-only, and used in RTS/CTS hardware handshaking.  Typically, an
        /// attached modem sets the line to indicate that transmission can proceed.
        /// </remarks>
        public bool CtsHolding
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.CtsHolding;
            }
        }

        /// <summary>
        /// Gets a value indicating whether SerialPort transmission is currently
        /// in a break state.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public bool InBreak
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.BreakState;
            }
        }

        /// <summary>
        /// Gets or sets the number of data bits per serial byte.
        /// Defaults to 8 but must be in the range of [5,8].
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public int DataBits
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.DataBits;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.DataBits = value;
            }
        } // DataBits

        /// <summary>
        /// Gets or sets the method of setting/reading parity bit on each byte
        /// serial I/O.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public Parity Parity
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.Parity;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.Parity = value;
            }
        } // Parity

        /// <summary>
        /// Gets or sets the number of stop bits per serial byte.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public StopBits StopBits
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.StopBits;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.StopBits = value;
            }
        } // StopBits

        /// <summary>
        /// Gets or sets the protocol for flow control.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public Handshake Handshake
        {
            get
            {
                Debug.Assert(this.port != null, "port must not be null!");
                return this.port.Handshake;
            }

            set
            {
                Debug.Assert(this.port != null, "port must not be null!");
                this.port.Handshake = value;
            }
        } // Handshake

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine methods.
        /// </summary>
        public string NewLine
        {
            get
            {
                return this.port.NewLine;
            }

            [SuppressMessage("Microsoft.Usage", 
                "CA2208:InstantiateArgumentExceptionsCorrectly",
                Justification = "Ok here")]
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("InvalidNullEmptyArgument");
                } // if

                this.port.NewLine = value;
            }
        } // NewLine

        /// <summary>
        /// Gets the number of byte of data in the receive buffer.
        /// </summary>
        public int BytesToRead
        {
            get
            {
                return this.port.BytesToRead;
            }
        } // BytesToRead

        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        public int BytesToWrite
        {
            get
            {
                return this.port.BytesToWrite;
            }
        } // BytesToWrite

        /// <summary>
        /// Gets or sets the size of the serial port input buffer.
        /// </summary>
        public int ReadBufferSize
        {
            get
            {
                return this.port.ReadBufferSize;
            }

            set
            {
                this.port.ReadBufferSize = value;
            }
        } // ReadBufferSize

        /// <summary>
        /// Gets or sets the size of the serial port output buffer. 
        /// </summary>
        public int WriteBufferSize
        {
            get
            {
                return this.port.WriteBufferSize;
            }

            set
            {
                this.port.WriteBufferSize = value;
            }
        } // WriteBufferSize
        #endregion // ILOWLEVELCOM PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="LowLevelSerialPort"/> class.
        /// </summary>
        public LowLevelSerialPort()
        {
            this.port = new SerialPort();
            this.port.DataReceived += this.PortDataReceived;
            this.port.ErrorReceived += this.PortErrorReceived;
            this.port.PinChanged += this.PortPinChanged;
        } // LowLevelSerialPort()
        #endregion // CONSTRUCTION

        //// -----------------------------------------------------------------

        #region SERIAL PORT EVENT HANDLING
        /// <summary>
        /// Handles the data received event of a SerialPort object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SerialDataReceivedEventArgs"/> 
        /// instance containing the event data.</param>
        private void PortDataReceived(object sender, 
            SerialDataReceivedEventArgs e)
        {
            if (this.LowLevelComEvent != null)
            {
                var args = new LowLevelComEventArgs(
                  LowLevelComEventType.DataReceived, e);
                this.LowLevelComEvent(this, args);
            } // if
        } // PortDataReceived()

        /// <summary>
        /// Handles the error received event of a SerialPort object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SerialErrorReceivedEventArgs"/>
        /// instance containing the event data.</param>
        private void PortErrorReceived(object sender, 
            SerialErrorReceivedEventArgs e)
        {
            if (this.LowLevelComEvent != null)
            {
                var args = new LowLevelComEventArgs(
                  LowLevelComEventType.ErrorReceived, e);
                this.LowLevelComEvent(this, args);
            } // if
        } // PortErrorReceived()

        /// <summary>
        /// Handles the pin changed event of a SerialPort object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SerialPinChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void PortPinChanged(object sender, 
            SerialPinChangedEventArgs e)
        {
            if (this.LowLevelComEvent != null)
            {
                var args = new LowLevelComEventArgs(
                  LowLevelComEventType.SerialPinChange, e);
                this.LowLevelComEvent(this, args);
            } // if
        } // PortPinChanged()
        #endregion SERIAL PORT EVENT HANDLING

        //// ---------------------------------------------------------------------

        #region ILOWLEVELCOM METHODS
        /// <summary>
        /// Opens (synchronously) a serial connection.
        /// </summary>
        public void Open()
        {
            this.port.Open();
        } // Open()

        /// <summary>
        /// Opens (asynchronously) a serial connection.
        /// </summary>
        /// <param name="userCallback">Callback method to notify application</param>
        public void Open(LowLevelAsyncCallback userCallback)
        {
            // asynchronous mode currently not supported, use default
            this.Open();
        } // Open()

        /// <summary>
        /// Closes (synchronously) the current serial connection.
        /// </summary>
        public void Close()
        {
            this.port.Close();
        } // Close()

        /// <summary>
        /// Returns -1 on timeout, or the first available full char if found before.
        /// </summary>
        /// <returns>
        /// -1 on timeout, or the first available full char if found before.
        /// </returns>
        public int Read()
        {
            return this.port.ReadByte();
        } // Read()

        /// <summary>
        /// Reads (synchronously) from the current TCP connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <returns>The number of bytes received.</returns>
        public int Read(ref byte[] buffer, int offset, int len)
        {
            return this.port.Read(buffer, offset, len);
        } // Read()

        /// <summary>
        /// Reads (asynchronously) from the current serial connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        public void Read(ref byte[] buffer, int offset, int len,
          LowLevelAsyncReadCallback userCallback)
        {
            // asynchronous mode currently not supported, use default
            this.port.Read(buffer, offset, len);
        } // Read()

        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer.</returns>
        public string ReadExisting()
        {
            return this.port.ReadExisting();
        } // ReadAvailable()

        /// <summary>
        /// Writes (synchronously) to the current serial connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Write(string data)
        {
            this.port.Write(data);
        } // Write()

        /// <summary>
        /// Writes (synchronously) to the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that containing the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        public void Write(byte[] buffer, int offset, int len)
        {
            this.port.Write(buffer, offset, len);
        } // Write()

        /// <summary>
        /// Writes (asynchronously) to the current serial connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        public void Write(string data, LowLevelAsyncCallback userCallback)
        {
            // asynchronous mode currently not supported, use default
            this.port.Write(data);
        } // Write()

        /// <summary>
        /// Writes (asynchronously) to the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that containing the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        public void Write(byte[] buffer, int offset, int len,
          LowLevelAsyncCallback userCallback)
        {
            // asynchronous mode currently not supported, use default
            this.port.Write(buffer, offset, len);
        } // Write()

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        public void WriteLine(string text)
        {
            this.port.Write(text + this.NewLine);
        } // WriteLine()

        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        public void DiscardInBuffer()
        {
            this.port.DiscardInBuffer();
        } // DiscardInBuffer()

        /// <summary>
        /// Discards data from the low level communication send buffer.
        /// </summary>
        public void DiscardOutBuffer()
        {
            this.port.DiscardOutBuffer();
        } // DiscardOutBuffer()

        /// <summary>
        /// Returns a string representing this object.
        /// </summary>
        /// <returns>A string representing this object</returns>
        public override string ToString()
        {
            string str = string.Format(
              CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}, {4}",
              this.PortName, this.BaudRate, this.DataBits, this.Parity,
              this.StopBits);

            return str;
        } // ToString()
        #endregion // ILOWLEVELCOM METHODS

        //// ---------------------------------------------------------------------

        #region IDISPOSABLE METHODS
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
                    this.port.Dispose();
                } // if

                // Note disposing has been done.
                this.disposed = true;
            } // if
        } // Dispose()
        #endregion // IDISPOSABLE METHODS
    } // LowLevelSerialPort
} // Tethys.OBD.ObdAnalyzer.Net.Core

// ============================
// End of LowLevelSerialPort.cs
// ============================
