#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdSupport.cs" company="Tethys">
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
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Support methods for OBD.
    /// </summary>
    public class ObdSupport
    {
        /// <summary>
        /// Converts the specified byte value to a hex string.
        /// </summary>
        /// <param name="b">The byte value.</param>
        /// <returns>A hex string.</returns>
        public static string ByteToHex(byte b)
        {
            return string.Format("{0:X2}", b);
        } // ByteToHex()

        /// <summary>
        /// Converts the specified 16 bit integer value to a hex string.
        /// </summary>
        /// <param name="val">The 16 bit integer value.</param>
        /// <param name="addSpaces">if set to <c>true</c> add spaces.</param>
        /// <returns>
        /// A hex string.
        /// </returns>
        public static string Int16ToHex(short val, bool addSpaces)
        {
            var result = string.Format("{0:X4}", val);

            if (addSpaces)
            {
                result = result.Insert(2, " ");
            } // if

            return result;
        } // Int16ToHex()

        /// <summary>
        /// Converts the specified 32 bit integer value to a hex string.
        /// </summary>
        /// <param name="val">The 32 bit integer value.</param>
        /// <param name="addSpaces">if set to <c>true</c> add spaces.</param>
        /// <returns>
        /// A hex string.
        /// </returns>
        public static string Int32ToHex(int val, bool addSpaces)
        {
            var result = string.Format("{0:X8}", val);

            if (addSpaces)
            {
                result = result.Insert(6, " ");
                result = result.Insert(4, " ");
                result = result.Insert(2, " ");
            } // if

            return result;
        } // Int32ToHex()

        /// <summary>
        /// Converts the specified text to an integer value
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="removeSpaces">if set to <c>true</c> remove any spaces.</param>
        /// <returns>An unsigned integer value.</returns>
        public static uint HexToInt(string text, bool removeSpaces = true)
        {
            if (removeSpaces)
            {
                text = text.Replace(" ", string.Empty);
            } // if

            uint result;
            if (!uint.TryParse(text, NumberStyles.HexNumber, 
                CultureInfo.InvariantCulture, out result))
            {
                return 0;
            }

            return result;
        } // HexToInt()

        /// <summary>
        /// Gets the engine load.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The engine load.</returns>
        public static byte GetCalculatedEngineLoad(string text)
        {
            return (byte)((HexToInt(text) * 100) / 255);
        } // GetCalculatedEngineLoad()

        /// <summary>
        /// Gets the engine coolant temperature.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The engine coolant temperature.</returns>
        public static byte GetEngineCoolantTemperature(string text)
        {
            return (byte)(HexToInt(text) - 40);
        } // GetEngineCoolantTemperature()

        /// <summary>
        /// Gets the fuel trim value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The fuel trim value.</returns>
        public static float GetFuelTrimValue(string text)
        {
            var val = ((int)HexToInt(text) - 128) * 100;
            return (float)val / 128;
        } // GetFuelTrimValue()

        /// <summary>
        /// Gets the engine RPM.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The engine RPM.</returns>
        public static short GetEngineRpm(string text)
        {
            var all = HexToInt(text);

            return (short)(all / 4);
        } // GetEngineRpm()

        /// <summary>
        /// Gets the vehicle speed.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The vehicle speed.</returns>
        public static byte GetVehicleSpeed(string text)
        {
            return (byte)(HexToInt(text));
        } // GetVehicleSpeed()

        /// <summary>
        /// Gets the MAF air flow rate.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The MAF air flow rate.</returns>
        public static float GetMafAirFlowRate(string text)
        {
            return ((float)HexToInt(text)) / 100;
        } // GetMafAirFlowRate()

        /// <summary>
        /// Gets the throttle position.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The throttle position.</returns>
        public static float GetThrottlePosition(string text)
        {
            return ((float)HexToInt(text) * 100) / 255;
        } // GetThrottlePosition()

        /// <summary>
        /// Gets the oxygen sensor PID for the given bank and sensor.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="sensor">The sensor.</param>
        /// <returns>The oxygen sensor PID for the given bank and sensor.</returns>
        public static int GetOxygenSensorPid(int bank, int sensor)
        {
            var pid = 0x14 + ((bank - 1) * 4) + (sensor - 1);
            return pid;
        } // GetOxygenSensorPid()

        /// <summary>
        /// Gets the oxygen sensors present text.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>The oxygen sensors present text.</returns>
        public static string GetOxygenSensorsPresentText(byte val)
        {
            var result = string.Empty;
            var bank1 = (byte)(val & 0x0f);
            if (bank1 > 0)
            {
                result = "Bank 1, sensors " + GetOxygenSensorsPresentSensorText(bank1);
            } // if
            var bank2 = (byte)(val >> 4);
            if (bank2 > 0)
            {
                if (result.Length > 0)
                {
                    result += ", ";
                } // if
                result += "Bank 2, sensors " + GetOxygenSensorsPresentSensorText(bank2);
            } // if

            return result;
        } // GetOxygenSensorsPresentText()

        /// <summary>
        /// Gets the oxygen sensors present sensor text.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>The oxygen sensors present sensor text.</returns>
        public static string GetOxygenSensorsPresentSensorText(byte val)
        {
            var sb = new StringBuilder(10);

            var check = 1;
            var count = 1;
            while (check < 9)
            {
                if ((val & check) > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    } // if
                    sb.AppendFormat("{0}", count);
                } // if
                count++;
                check = check << 1;
            } // while

            return sb.ToString();
        } // GetOxygenSensorsPresentSensorText()

        /// <summary>
        /// Gets the obd standard text.
        /// </summary>
        /// <param name="standard">The standard.</param>
        /// <returns>The text.</returns>
        public static string GetObdStandardText(byte standard)
        {
            switch (standard)
            {
                case 1:
                    return "OBD-II as defined by the CARB";
                case 2:
                    return "OBD as defined by the EPA";
                case 3:
                    return "OBD and OBD-II";
                case 4:
                    return "OBD-I";
                case 5:
                    return "Not OBD compliant";
                case 6:
                    return "EOBD (Europe)";
                case 7:
                    return "EOBD and OBD-II";
                case 8:
                    return "EOBD and OBD";
                case 9:
                    return "EOBD, OBD and OBD II";
                case 10:
                    return "JOBD (Japan)";
                case 11:
                    return "JOBD and OBD II";
                case 12:
                    return "JOBD and EOBD";
                case 13:
                    return "JOBD, EOBD, and OBD II";
                case 17:
                    return "Engine Manufacturer Diagnostics (EMD)";
                case 18:
                    return "Engine Manufacturer Diagnostics Enhanced (EMD+)";
                case 19:
                    return "Heavy Duty On-Board Diagnostics (Child/Partial) (HD OBD-C)";
                case 20:
                    return "Heavy Duty On-Board Diagnostics (HD OBD)";
                case 21:
                    return "World Wide Harmonized OBD (WWH OBD)";
                case 23:
                    return "Heavy Duty Euro OBD Stage I without NOx control (HD EOBD-I)";
                case 24:
                    return "Heavy Duty Euro OBD Stage I with NOx control (HD EOBD-I N)";
                case 25:
                    return "Heavy Duty Euro OBD Stage II without NOx control (HD EOBD-II)";
                case 26:
                    return "Heavy Duty Euro OBD Stage II with NOx control (HD EOBD-II N)";
                case 28:
                    return "Brazil OBD Phase 1 (OBDBr-1)";
                case 29:
                    return "Brazil OBD Phase 2 (OBDBr-2)";
                case 30:
                    return "Korean OBD (KOBD)";
                case 31:
                    return "India OBD I (IOBD I)";
                case 32:
                    return "India OBD II (IOBD II)";
                case 33:
                    return "Heavy Duty Euro OBD Stage VI (HD EOBD-IV)";
                default:
                    return string.Format("Unknown ({0})", standard);
            } // switch
        } // GetObdStandardText()

        /// <summary>
        /// Gets the catalyst temperature pid for the given bank and sensor.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="sensor">The sensor.</param>
        /// <returns>The PID.</returns>
        public static int GetCatalystTemperaturePid(int bank, int sensor)
        {
            var pid = 0x3C + ((bank - 1) * 2) + sensor - 1;
            return pid;
        } // GetCatalystTemperaturePid()

        /// <summary>
        /// Determines whether the specified PID is supported.
        /// </summary>
        /// <param name="pidBitCode">The pid bit code.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>
        ///   <c>true</c> if the PID is supported; otherwise <c>false</c>.
        /// </returns>
        public static bool IsPidSupported(uint pidBitCode, int pid, int offset)
        {
            var pos = 0x20 - (pid - offset);

            if ((pos < 0) || (pos > 0x20))
            {
                throw new ArgumentOutOfRangeException("pid");
            } // if

            var check = 1 << pos;
            var result = (pidBitCode & check) > 0;

            return result;
        } // IsPidSupported()

        /// <summary>
        /// Determines whether the specified PID is supported.
        /// </summary>
        /// <param name="pidBitCode">The pid bit code.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>
        ///   A text of the supported PIDs.
        /// </returns>
        public static string GetSupportedPidText(uint pidBitCode, int offset)
        {
            var sb = new StringBuilder(100);

            for (var pid = 0; pid < 0x20; pid++)
            {
                var check = 1 << (0x1F - pid);
                if ((pidBitCode & check) > 0)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, 
                        "0x{0:X2}, ", pid + 1 + offset);
                } // if
            } // for

            if (sb.Length == 0)
            {
                return "None";
            } // if

            return sb.ToString(0, sb.Length - 2);
        } // IsPidSupported()

        /// <summary>
        /// Gets the multiline CAN string from the given data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="cmdPrefix">The command prefix.</param>
        /// <param name="expectedLength">The expected length.</param>
        /// <returns>
        /// A text string.
        /// </returns>
        public static string GetMultilineCanString(string data, string cmdPrefix,
            int expectedLength)
        {
            var rawData = data;
            var sb = new StringBuilder(50);
            var length = 0;
            
            var index = data.IndexOf(':');
            if (index < 0)
            {
                // invalid or unknown format
                return rawData;
            } // if

            data = data.Substring(index + 1, data.Length - index - 1);
            if (!data.StartsWith(cmdPrefix))
            {
                // invalid or unknown format
                return rawData;
            } // if
            length += cmdPrefix.Length / 2;

            var help = data.Substring(cmdPrefix.Length, 2);
            int numDataItems;
            try
            {
                numDataItems = int.Parse(help, NumberStyles.HexNumber);
            }
            catch
            {
                // invalid or unknown format
                return rawData;
            } // if

            if (numDataItems != 1)
            {
                // invalid or unknown format
                return rawData;
            } // if

            length++;
            index = cmdPrefix.Length + 2;
            int nullChars = 0;
            while (index + 2 <= data.Length)
            {
                var ch = GetChar(data.Substring(index, 2));
                if ((ch != 0xff) && (ch != 0))
                {
                    sb.Append(ch);
                    length++;
                } // if
                if (ch == 0)
                {
                    nullChars++;
                } // if

                index += 2;
            } // while

#if false
            if (sb.Length + nullChars != length - 3)
            {
                // invalid or unknown format
                return rawData;
            } // if
#endif

            return sb.ToString();
        } // GetMultilineCanString()

        /// <summary>
        /// Gets an ASCII character from the given hex string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A <see cref="char"/>.</returns>
        public static char GetChar(string text)
        {
            try
            {
                var ascii = int.Parse(text, NumberStyles.HexNumber);
                return (char)ascii;
            }
            catch
            {
                // invalid or unknown format
                return (char)0xff;
            } // if
        } // GetChar()

        /// <summary>
        /// Gets the monitor status text.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The text.</returns>
        public static string GetMonitorStatusText(uint status)
        {
            var sb = new StringBuilder(200);

            sb.AppendFormat("  MIL is {0}\r\n", 
                    ((status & ObdBase.B7) > 0) ? "on" : "off");

            var numDtcs = (status & 0x7F000000) >> 24;
            sb.AppendFormat("  DTCs available: {0}\r\n", numDtcs);

            if ((status & ObdBase.B2) > 0)
            {
                sb.AppendFormat("  Components test is {0}\r\n",
                    ((status & ObdBase.B6) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.B1) > 0)
            {
                sb.AppendFormat("  Fuel system test is {0}\r\n",
                    ((status & ObdBase.B5) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.B0) > 0)
            {
                sb.AppendFormat("  Misfire test is {0}\r\n",
                    ((status & ObdBase.B4) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.B3) > 0)
            {
                sb.Append("  Compression ignition monitors supported:\r\n");
                if ((status & ObdBase.C7) > 0)
                {
                    sb.AppendFormat("  EGR and/or VVT system test is {0}\r\n",
                        ((status & ObdBase.D7) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C6) > 0)
                {
                    sb.AppendFormat("  PM filter monitoring test is {0}\r\n",
                        ((status & ObdBase.D6) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C5) > 0)
                {
                    sb.AppendFormat("  Exhaust Gas Sensor test is {0}\r\n",
                        ((status & ObdBase.D5) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C3) > 0)
                {
                    sb.AppendFormat("  Boost Pressure test is {0}\r\n",
                        ((status & ObdBase.D3) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C1) > 0)
                {
                    sb.AppendFormat("  NOx/SCR Monitor test is {0}\r\n",
                        ((status & ObdBase.D1) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C0) > 0)
                {
                    sb.AppendFormat("  NMHC Catalyst test is {0}\r\n",
                        ((status & ObdBase.D0) > 0) ? "incomplete" : "complete");
                } // if
            }
            else
            {
                sb.Append("  Spark ignition monitors supported:\r\n");
                if ((status & ObdBase.C7) > 0)
                {
                    sb.AppendFormat("  EGR system test is {0}\r\n",
                        ((status & ObdBase.D7) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C6) > 0)
                {
                    sb.AppendFormat("  Oxygen sensor heater test is {0}\r\n",
                        ((status & ObdBase.D6) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C5) > 0)
                {
                    sb.AppendFormat("  Oxygen sensor test is {0}\r\n",
                        ((status & ObdBase.D5) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C4) > 0)
                {
                    sb.AppendFormat("  A/C refrigerant test is {0}\r\n",
                        ((status & ObdBase.D4) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C3) > 0)
                {
                    sb.AppendFormat("  Secondary air system test is {0}\r\n",
                        ((status & ObdBase.D3) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C2) > 0)
                {
                    sb.AppendFormat("  Evaporative system test is {0}\r\n",
                        ((status & ObdBase.D2) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C1) > 0)
                {
                    sb.AppendFormat("  Heated catalyst test is {0}\r\n",
                        ((status & ObdBase.D1) > 0) ? "incomplete" : "complete");
                } // if

                if ((status & ObdBase.C0) > 0)
                {
                    sb.AppendFormat("  Catalyst test is {0}\r\n",
                        ((status & ObdBase.D0) > 0) ? "incomplete" : "complete");
                } // if
            } // if

            var length = (sb.Length > 2) ? sb.Length - 2 : sb.Length;
            return sb.ToString(0, length);
        } // GetMonitorStatusText()

        /// <summary>
        /// Gets the monitor status drive cycle text.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The text.</returns>
        public static string GetMonitorStatusDriveCycleText(uint status)
        {
            var sb = new StringBuilder(200);

            if ((status & ObdBase.B2) > 0)
            {
                sb.AppendFormat("  Components test is {0}\r\n", 
                    ((status & ObdBase.B6) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.B1) > 0)
            {
                sb.AppendFormat("  Fuel system test is {0}\r\n",
                    ((status & ObdBase.B5) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.B0) > 0)
            {
                sb.AppendFormat("  Misfire test is {0}\r\n",
                    ((status & ObdBase.B4) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C7) > 0)
            {
                sb.AppendFormat("  EGR system test is {0}\r\n",
                    ((status & ObdBase.D7) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C6) > 0)
            {
                sb.AppendFormat("  Oxygen sensor heater test is {0}\r\n",
                    ((status & ObdBase.D6) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C5) > 0)
            {
                sb.AppendFormat("  Oxygen sensor test is {0}\r\n",
                    ((status & ObdBase.D5) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C4) > 0)
            {
                sb.AppendFormat("  A/C refrigerant test is {0}\r\n",
                    ((status & ObdBase.D4) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C3) > 0)
            {
                sb.AppendFormat("  Secondary air system test is {0}\r\n",
                    ((status & ObdBase.D3) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C2) > 0)
            {
                sb.AppendFormat("  Evaporative system test is {0}\r\n",
                    ((status & ObdBase.D2) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C1) > 0)
            {
                sb.AppendFormat("  Heated catalyst test is {0}\r\n",
                    ((status & ObdBase.D1) > 0) ? "incomplete" : "complete");
            } // if

            if ((status & ObdBase.C0) > 0)
            {
                sb.AppendFormat("  Catalyst test is {0}\r\n",
                    ((status & ObdBase.D0) > 0) ? "incomplete" : "complete");
            } // if

            var length = (sb.Length > 2) ? sb.Length - 2 : sb.Length;
            return sb.ToString(0, length);
        } // GetMonitorStatusDriveCycleText()

        /// <summary>
        /// Gets the DTC text from the BCD coded data.
        /// </summary>
        /// <param name="dtc">The DTC.</param>
        /// <returns>The DTC text.</returns>
        public static string GetDtcText(ushort dtc)
        {
            var dtcChar = new[] { "P", "C", "B", "U" };

            var first = dtcChar[(dtc & 0xC000) >> 14];

            var second = (dtc & 0x3000) >> 12;
            var third = (dtc & 0x0F00) >> 8;
            var remaining = (dtc & 0x00ff);

            var text = string.Format("{0}{1}{2:X}{3:X2}", 
                first, second, third, remaining);
            return text;
        } // GetDtcText()

        /// <summary>
        /// Returns the last answer in case the given text contains more than
        /// one OBD answer.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The last answer.</returns>
        public static string GetLastAnswer(string text)
        {
            var last = text.LastIndexOf(ObdBase.ElmPrompt, 
                StringComparison.OrdinalIgnoreCase);
            if (last < 1)
            {
                // no end, return complete string
                return text;
            } // if

            var start = text.LastIndexOf(ObdBase.ElmPrompt, last - 1,
                StringComparison.OrdinalIgnoreCase);
            if (start < 0)
            {
                // no previous, return complete string
                return text;
            } // if

            return text.Substring(start + 1);
        } // GetLastAnswer()
    } // ObdSupport

//////////////////////////////////////////////////
/* 09 00 -> 01 30 00 00 00 capabilites  5 bytes??? 

10 Start Diagnostic Session
11 ECU Reset
12 Read Freeze Frame Data
13 Read Diagnostic Trouble Codes
14 Clear Diagnostic Information
17 Read Status Of Diagnostic Trouble Codes
18 Read Diagnostic Trouble Codes By Status
1A Read Ecu Id
20 Stop Diagnostic Session
21 Read Data By Local Id
22 Read Data By Common Id
23 Read Memory By Address
25 Stop Repeated Data Transmission
26 Set Data Rates
27 Security Access
2C Dynamically Define Local Id
2E Write Data By Common Id
2F Input Output Control By Common Id
30 Input Output Control By Local Id
31 Start Routine By Local ID
32 Stop Routine By Local ID
33 Request Routine Results By Local Id
34 Request Download
35 Request Upload
36 Transfer data
37 Request transfer exit
38 Start Routine By Address
39 Stop Routine By Address
3A Request Routine Results By Address
3B Write Data By Local Id
3D Write Memory By Address
3E Tester Present
81 -> xx xx Start Communication
82 Stop Communication
83 Access Timing Parameters
85 Start Programming Mode
     * 
     * 
     * Response Failure Codes

10 General Reject
11 Service Not Supported
12 Sub Function Not Supported - Invalid Format
21 Busy - repeat Request
22 Conditions Not Correct Or Request Sequence Error
23 Routine Not Complete Or Service In Progress
31 Request Out Of Range
33 Security Access Denied - security Access Requested
35 Invalid Key
36 Exceed Number Of Attempts
37 Required Time Delay Not Expired
40 Download Not Accepted
41 Improper Download Type
42 Can Not Download To Specified Address
43 Can Not Download Number Of Bytes Requested
50 Upload Not Accepted
51 Improper Upload Type
52 Can Not Upload From Specified Address
53 Can Not Upload Number Of Bytes Requested
71 Transfer Suspended
72 Transfer Aborted
74 Illegal Address In Block Transfer
75 Illegal Byte Count In Block Transfer
76 Illegal Block Trasnfer Type
77 Block Transfer Data Checksum Error
78 Request Correcty Rcvd - Rsp Pending
79 Incorrect Byte Count During Block Transfer
80 Service Not Supported In Active Diagnostic Mode
C1 Start Comms +ve response
C2 Stop Comms +ve response
C3 Access Timing Params +ve response
81-8F Reserved
90-F9 Vehicle manufacturer specific
FA-FE System supplier specific
FF Reserved by document
     */
} // TgSoft.OBD
