#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="Mode.cs" company="Tethys">
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
    /// <summary>
    /// OBD II Modes.
    /// Source = <a href="http://en.wikipedia.org/wiki/OBD-II_PIDs">Wikipedia</a>.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Unknown mode (not part of OBD standard).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 01 - Show current data.
        /// </summary>
        Mode01 = 0x01,

        /// <summary>
        /// 02 - Show freeze frame data.
        /// </summary>
        Mode02 = 0x02,

        /// <summary>
        /// 03 - Show stored Diagnostic Trouble Codes.
        /// </summary>
        Mode03 = 0x03,

        /// <summary>
        /// 04 - Clear Diagnostic Trouble Codes and stored values.
        /// </summary>
        Mode04 = 0x04,

        /// <summary>
        /// 05 - Test results, oxygen sensor monitoring (non CAN only).
        /// </summary>
        Mode05 = 0x05,

        /// <summary>
        /// 06 - Test results, other component/system monitoring (Test results,
        /// oxygen sensor monitoring for CAN only).
        /// </summary>
        Mode06 = 0x06,

        /// <summary>
        /// 07 - Show pending Diagnostic Trouble Codes (detected during current 
        /// or last driving cycle)
        /// </summary>
        Mode07 = 0x07,

        /// <summary>
        /// 08 - Control operation of on-board component/system.
        /// </summary>
        Mode08 = 0x08,

        /// <summary>
        /// 09 - Request vehicle information.
        /// </summary>
        Mode09 = 0x09,

        /// <summary>
        /// 0A - Permanent Diagnostic Trouble Codes (DTCs) (Cleared DTCs).
        /// </summary>
        Mode10 = 0x0a,
    } // Mode
} // TgSoft.OBD
