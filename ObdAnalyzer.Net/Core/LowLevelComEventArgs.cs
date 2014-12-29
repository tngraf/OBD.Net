#region Header
// ---------------------------------------------------------------------------
// S e r i a l P o r t S u p p o r t
// ===========================================================================
//
// This library contains common code for Tethys projects.
//
// ===========================================================================
// <copyright file="LowLevelComEventArgs.cs" company="Tethys">
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

    /// <summary>
    /// Low level communication events
    /// </summary>
    public enum LowLevelComEventType
    {
        /// <summary>
        /// LowLevelCom object has received data.
        /// </summary>
        DataReceived,

        /// <summary>
        /// LowLevelCom object has received an error.
        /// </summary>
        ErrorReceived,

        /// <summary>
        /// LowLevelCom object has detected a port pin change.
        /// </summary>
        SerialPinChange
    } // LowLevelComEventType

    /// <summary>
    /// This class implements low level communication events.
    /// </summary>
    public class LowLevelComEventArgs : EventArgs
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Love level COM event.
        /// </summary>
        private readonly LowLevelComEventType comEvent;

        /// <summary>
        /// Additional object data.
        /// </summary>
        private readonly object data;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets the type of low level communication event.
        /// </summary>
        public LowLevelComEventType EventType
        {
            get
            {
                return this.comEvent;
            }
        } // EventType

        /// <summary>
        /// Gets the specific low level event data.
        /// </summary>
        public object EventData
        {
            get
            {
                return this.data;
            }
        } // EventType
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="LowLevelComEventArgs"/> class.
        /// </summary>
        /// <param name="eventCode">event type</param>
        public LowLevelComEventArgs(LowLevelComEventType eventCode)
        {
            this.comEvent = eventCode;
        } // LowLevelComEventArgs()

        /// <summary>
        /// Initializes a new instance of the <see cref="LowLevelComEventArgs"/> class.
        /// </summary>
        /// <param name="eventCode">event type</param>
        /// <param name="data">event data</param>
        public LowLevelComEventArgs(LowLevelComEventType eventCode, object data)
        {
            this.comEvent = eventCode;
            this.data = data;
        } // LowLevelComEventArgs()
        #endregion CONSTRUCTION
    } // LowLevelComEventArgs
} // Tethys.OBD.ObdAnalyzer.Net.Core

// ==============================
// End of LowLevelComEventArgs.cs
// ==============================
