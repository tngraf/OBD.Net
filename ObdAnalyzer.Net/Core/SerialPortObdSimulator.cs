#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="SerialPortObdSimulator.cs" company="Tethys">
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
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Text;

    using Tethys.Logging;
    using Tethys.OBD.ObdSimulator;

    /// <summary>
    /// Simulates a serial port that is attached to an OBD device.
    /// </summary>
    public class SerialPortObdSimulator : ObdSimulator, ILowLevelCom
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Logger for this class.
        /// </summary>
        private static readonly ILog Log
            = LogManager.GetLogger(typeof(ObdManager));

        /// <summary>
        /// Output queue.
        /// </summary>
        private readonly Queue<string> queue;

        /// <summary>
        /// Flag 'port is open'.
        /// </summary>
        private bool open;

        /// <summary>
        /// Disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Current command.
        /// </summary>
        private string command;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region ILOWLEVELCOM MEMBERS
        /// <summary>
        /// Represents the method that handles low level communication events.
        /// </summary>
        public event EventHandler<LowLevelComEventArgs> LowLevelComEvent;

        //// ---------------------------------------------------------------------

        #region PUBLIC ILOWLEVELCOM PROPERTIES
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
            get { return this.open; }
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
                return Encoding.ASCII;
            }

            set
            {
            }
        }

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
                return -1;
            }

            set
            {
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
                return -1;
            }

            set
            {
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
                return false;
            }

            set
            {
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
                return false;
            }

            set
            {
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
                return "Simulator";
            }

            set
            {
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
                return 38400;
            }

            set
            {
            }
        }

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
        public bool CDHolding
        {
            get { return true; }
        }

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
        public bool CtsHolding
        {
            get { return true; }
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
            get { return false; }
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
                return 8;
            }

            set
            {
            }
        }

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
                return Parity.None;
            }

            set
            {
            }
        }

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
                return StopBits.One;
            }

            set
            {
            }
        }

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
                return Handshake.None;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the 
        /// ReadLine and WriteLine methods.
        /// </summary>
        /// <remarks>
        /// Used by LowLevelSerialPort
        /// </remarks>
        public string NewLine
        {
            get
            {
                return "\n";
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets the number of byte of data in the receive buffer.
        /// </summary>
        public int BytesToRead
        {
            get
            {
                return 0;
            }
        } // BytesToRead

        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        public int BytesToWrite
        {
            get
            {
                return 0;
            }
        } // BytesToWrite

        /// <summary>
        /// Gets or sets the size of the serial port input buffer.
        /// </summary>
        public int ReadBufferSize
        {
            get
            {
                return 0;
            }

            set
            {
            }
        } // ReadBufferSize

        /// <summary>
        /// Gets or sets the size of the serial port output buffer. 
        /// </summary>
        public int WriteBufferSize
        {
            get
            {
                return 0;
            }

            set
            {
            }
        } // WriteBufferSize
        #endregion // PUBLIC ILOWLEVELCOM PROPERTIES

        #endregion

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SerialPortObdSimulator"/> class.
        /// </summary>
        public SerialPortObdSimulator()
        {
            this.queue = new Queue<string>(20);
        } // SerialPortObdSimulator()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC ILOWLEVELCOM METHODS
        /// <summary>
        /// Opens (synchronously) a connection.
        /// </summary>
        public void Open()
        {
            this.open = true;
            Log.Debug("Virtual port opened.");
        } // Open()

        /// <summary>
        /// Opens (asynchronously) a connection.
        /// </summary>
        /// <param name="userCallback">Callback method to notify 
        /// application</param>
        public void Open(LowLevelAsyncCallback userCallback)
        {
            this.Open();
        } // Open()

        /// <summary>
        /// Closes (synchronously) the current connection.
        /// </summary>
        public void Close()
        {
            this.open = false;
            if (this.ShowLogging)
            {
                Log.Debug("Virtual port closed.");
            } // if
        } // Close()

        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in the 
        /// input buffer.
        /// </summary>
        /// <returns>
        /// The contents of the input buffer.
        /// </returns>
        public string ReadExisting()
        {
            var sb = new StringBuilder(20 * (this.queue.Count + 1));
            while (this.queue.Count > 0)
            {
                sb.Append(this.queue.Dequeue());
            } // while

#if MORE_LOGGING
            Log.DebugFormat("ReadExisting() = >>>{0}<<<", sb.ToString());
#endif

            return sb.ToString();
        } // ReadExisting()

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(string text)
        {
            this.ProcessData(text);
        } // Write

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        public void WriteLine(string text)
        {
            this.ProcessData(text + "\r\n");
        } // WriteLine()

        #region NOT IMPLEMENTED METHODS
        /// <summary>
        /// Returns -1 on timeout, or the first available full char if found
        /// before.
        /// </summary>
        /// <returns>
        /// -1 on timeout, or the first available full char if found before.
        /// </returns>
        public int Read()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads (synchronously) from the current TCP connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that is the storage 
        /// location for the received data.</param>
        /// <param name="offset">The zero-based position in the buffer parameter
        /// at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <returns>
        /// The number of bytes received.
        /// </returns>
        public int Read(ref byte[] buffer, int offset, int len)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads (asynchronously) from the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that is the storage 
        /// location for the received data.</param>
        /// <param name="offset">The zero-based position in the buffer parameter
        ///  at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        public void Read(ref byte[] buffer, int offset, int len, LowLevelAsyncReadCallback userCallback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes (synchronously) to the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that containing the data 
        /// to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter 
        /// at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        public void Write(byte[] buffer, int offset, int len)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes (asynchronously) to the current connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        public void Write(string data, LowLevelAsyncCallback userCallback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes (asynchronously) to the current connection.
        /// </summary>
        /// <param name="buffer">An array of type Byte that containing the data
        ///  to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter 
        /// at which to store the received data.</param>
        /// <param name="len">The number of bytes to receive.</param>
        /// <param name="userCallback">Callback method to notify application</param>
        public void Write(byte[] buffer, int offset, int len,
          LowLevelAsyncCallback userCallback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Discards data from the low level communication receive buffer.
        /// </summary>
        public void DiscardInBuffer()
        {
        }

        /// <summary>
        /// Discards data from the low level communication send buffer.
        /// </summary>
        public void DiscardOutBuffer()
        {
        }
        #endregion NOT IMPLEMENTED METHODS
        #endregion // PUBLIC ILOWLEVELCOM METHODS

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Processes the data.
        /// </summary>
        /// <param name="data">The data.</param>
        private void ProcessData(string data)
        {
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
        } // ProcessData()

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

            // ReSharper disable once RedundantNameQualifier
            if (retData == ObdSimulator.InvalidCommand)
            {
                if (this.ShowLogging)
                {
                    Log.ErrorFormat("Invalid command: '{0}'!", cmd);
                }
            }
            else
            {
                if (this.ShowLogging)
                {
                    Log.DebugFormat("Command '{0}' processed, answer = '{1}'", 
                        cmd, retData);
                } // if
                this.queue.Enqueue(retData);
                this.GenerateLowComEvent();
            } // if
        } // ProcessCommand()

        /// <summary>
        /// Generates a low level communication event.
        /// </summary>
        public void GenerateLowComEvent()
        {
            if (this.LowLevelComEvent != null)
            {
                var args = new LowLevelComEventArgs(
                  LowLevelComEventType.DataReceived, EventArgs.Empty);
                this.LowLevelComEvent(this, args);
            } // if
        } // GenerateLowComEvent()
        #endregion // PRIVATE METHODS

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
                    // nothing yet
                } // if

                // Note disposing has been done.
                this.disposed = true;
            } // if
        } // Dispose()
        #endregion // IDISPOSABLE METHODS
    } // SerialPortObdSimulator
}
