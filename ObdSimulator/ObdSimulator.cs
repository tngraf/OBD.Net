#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdSimulator.cs" company="Tethys">
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

namespace Tethys.OBD.ObdSimulator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Tethys.Logging;
    using Tethys.OBD;

    /// <summary>
    /// A simulator for OBD II commands.
    /// </summary>
    public class ObdSimulator : ObdBase
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Logger for this class.
        /// </summary>
        private static readonly ILog Log
            = LogManager.GetLogger(typeof(ObdSimulator));

        /// <summary>
        /// The mode 1 results.
        /// </summary>
        private Dictionary<int, string> mode1Results;

        /// <summary>
        /// The mode 5 results.
        /// </summary>
        private Dictionary<int, string> mode5Results;

        /// <summary>
        /// The mode 6 results.
        /// </summary>
        private Dictionary<int, string> mode6Results;

        /// <summary>
        /// The mode96 results.
        /// </summary>
        private Dictionary<int, string> mode9Results;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PROTECTED PROPERTIES
        /// <summary>
        /// Gets or sets a value indicating whether to echo.
        /// </summary>
        protected bool EchoOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use line feed.
        /// </summary>
        protected bool LineFeed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to print spaces.
        /// </summary>
        protected bool PrintSpaces { get; set; }

        /// <summary>
        /// Gets or sets the mode 3 answer.
        /// </summary>
        protected string Mode3Answer { get; set; }

        /// <summary>
        /// Gets or sets the mode 7 answer.
        /// </summary>
        protected string Mode7Answer { get; set; }
        #endregion // PROTECTED PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Return value for invalid command.
        /// </summary>
        public const string InvalidCommand = "INVALID";

        /// <summary>
        /// Gets or sets a value indicating whether to show log messages.
        /// </summary>
        public bool ShowLogging { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ObdSimulator"/> class.
        /// </summary>
        public ObdSimulator()
        {
            this.EchoOn = true;
            this.LineFeed = true;
            this.PrintSpaces = true;

            this.ShowLogging = true;

            // this.InitializeResults();
            // this.InitializeResultsAudiA3();
            this.InitializeResultsAudiA4();
        } // ObdSimulator()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        public void Reset()
        {
            this.EchoOn = true;
            this.LineFeed = true;
            this.PrintSpaces = true;
        } // Reset()

        /// <summary>
        /// Sets the engine load.
        /// </summary>
        /// <param name="load">The load.</param>
        public void SetEngineLoad(int load)
        {
            var hextext = string.Format(CultureInfo.InvariantCulture,
                "41 04 {0:X2}", (load * 255) / 100);
            if (this.mode1Results.ContainsKey(4))
            {
                this.mode1Results[0x04] = hextext;
            }
            else
            {
                this.mode1Results.Add(0x04, hextext);
            } // if
        } // SetEngineLoad()

        /// <summary>
        /// Sets the engine coolant temperature.
        /// </summary>
        /// <param name="temperature">The temperature.</param>
        public void SetEngineCoolantTemperature(int temperature)
        {
            var hextext = string.Format(CultureInfo.InvariantCulture,
                "41 05 {0:X2}", temperature + 40);
            if (this.mode1Results.ContainsKey(5))
            {
                this.mode1Results[0x05] = hextext;
            }
            else
            {
                this.mode1Results.Add(0x05, hextext);
            } // if
        } // SetEngineCoolantTemperature()

        /// <summary>
        /// Sets the engine RPM.
        /// </summary>
        /// <param name="rpm">The RPM.</param>
        public void SetEngineRpm(int rpm)
        {
            rpm = rpm * 4;
            var hextext = string.Format(CultureInfo.InvariantCulture,
                "41 0C {0:X2} {1:X2}", (rpm / 256), (rpm & 0xFF));
            if (this.mode1Results.ContainsKey(0x0C))
            {
                this.mode1Results[0x0C] = hextext;
            }
            else
            {
                this.mode1Results.Add(0x0C, hextext);
            } // if
        } // SetEngineRpm()

        /// <summary>
        /// Sets the vehicle speed.
        /// </summary>
        /// <param name="speed">The speed.</param>
        public void SetVehicleSpeed(int speed)
        {
            var hextext = string.Format(CultureInfo.InvariantCulture,
                "41 0D {0:X2}", speed);
            if (this.mode1Results.ContainsKey(0x0D))
            {
                this.mode1Results[0x0D] = hextext;
            }
            else
            {
                this.mode1Results.Add(0x0D, hextext);
            } // if
        } // SetVehicleSpeed()

        /// <summary>
        /// Sets a mode 1 PID raw value.
        /// </summary>
        /// <remarks>
        /// There will be no checks - the caller is responsible for
        /// a valid value.
        /// </remarks>
        /// <param name="pid">The pid.</param>
        /// <param name="value">The value.</param>
        public void SetPidRawValue(int pid, string value)
        {
            if (this.mode1Results.ContainsKey(pid))
            {
                this.mode1Results[pid] = value;
            }
            else
            {
                this.mode1Results.Add(pid, value);
            } // if
        } // SetPidRawValue()

        /// <summary>
        /// Gets a mode 1 PID raw value.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns>The answer string.</returns>
        public string GetPidRawValue(int pid)
        {
            var retData = this.mode1Results.ContainsKey(pid)
                ? this.mode1Results[pid] : ElmNoDataPrompt;

            return retData;
        } // GetPidRawValue()

        /// <summary>
        /// Processes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>An answer to this command.</returns>
        public string ProcessCommand(string command)
        {
            string retData;

            if ((string.IsNullOrEmpty(command)) || (command.Length < 2))
            {
                // invalid command
                return InvalidCommand;
            } // if

            var rawCommand = command;
            command = command.ToUpperInvariant();
            command = command.Replace(" ", string.Empty);

            var mode = command.Substring(0, 2);
            if (mode == ObdBase.ElmAtPrefix)
            {
                retData = this.ProcessAtCommand(command.Substring(2));
            }
            else
            {
                var pid = 0;
                var data = string.Empty;
                if ((mode == "01") || (mode == "02") || (mode == "09"))
                {
                    if (command.Length <= 2)
                    {
                        // invalid command
                        return InvalidCommand;
                    } // if

                    var pidText = command.Substring(2, 2);
                    
                    try
                    {
                        pid = byte.Parse(pidText, NumberStyles.HexNumber);
                    }
                    catch (Exception)
                    {
                        // invalid command pid
                        return InvalidCommand;
                    } // catch

                    data = command.Substring(4);
                } // if

                if ((mode == "05"))
                {
                    if (command.Length < 4)
                    {
                        // invalid command
                        return InvalidCommand;
                    } // if

                    var pidText = command.Substring(2, 4);

                    try
                    {
                        pid = int.Parse(pidText, NumberStyles.HexNumber);
                    }
                    catch (Exception)
                    {
                        // invalid command pid
                        return InvalidCommand;
                    } // catch

                    data = command.Substring(4);
                } // if

                switch (mode)
                {
                    case "01":
                        retData = this.ProcessMode1Command(pid, data);
                        break;
                    case "02":
                        retData = this.ProcessMode2Command(pid, data);
                        break;
                    case "03":
                        retData = this.ProcessMode3Command();
                        break;
                    case "04":
                        retData = this.ProcessMode4Command();
                        break;
                    case "05":
                        retData = this.ProcessMode5Command(pid, data);
                        break;
                    case "06":
                        retData = this.ProcessMode6Command();
                        break;
                    case "07":
                        retData = this.ProcessMode7Command();
                        break;
                    case "08":
                        retData = this.ProcessMode8Command(pid, data);
                        break;
                    case "09":
                        retData = this.ProcessMode9Command(pid, data);
                        break;
                    case "0A":
                        retData = this.ProcessMode10Command(pid, data);
                        break;
                    default:
                        // unknown command
                        return ObdBase.ElmQueryPrompt;
                } // switch
            } // if

            if (!string.IsNullOrEmpty(retData))
            {
                retData = retData + ObdBase.ElmCrLf + ObdBase.ElmCrLf + ObdBase.ElmPrompt;
            } // if

            if (this.EchoOn)
            {
                retData = rawCommand + ObdBase.ElmCrLf + retData;
            } // if

            return retData;
        } // ProcessCommand()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PROTECTED METHODS
        /// <summary>
        /// Processes an AT command.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessAtCommand(string data)
        {
            string retData;

            switch (data)
            {
                case "DP":
                    retData = "AUTO, ISO 15765-4 (CAN 11/500)";
                    break;
                case "E0":
                    retData = ObdBase.ElmOkPrompt;
                    this.HandleEcho(false);
                    break;
                case "E1":
                    retData = ObdBase.ElmOkPrompt;
                    this.HandleEcho(true);
                    break;
                case "I":
                    retData = "OBD-Simulator";
                    break;
                case "L0":
                    retData = ObdBase.ElmOkPrompt;
                    this.HandleLineFeed(false);
                    break;
                case "L1":
                    retData = ObdBase.ElmOkPrompt;
                    this.HandleLineFeed(true);
                    break;
                case "RV":
                    retData = "12.6V";
                    break;
                case "SP00": // Set Protocol to Auto and save it
                    retData = ObdBase.ElmOkPrompt;
                    break;
                case "WS": // similar to ATZ
                    retData = "OBD-Simulator";
                    if (this.ShowLogging)
                    {
                        Log.Debug("ATZ - reinitializing data...");
                    } // if
                    break;
                case "Z": // reset
                    retData = "OBD-Simulator";
                    if (this.ShowLogging)
                    {
                        Log.Debug("ATZ - reinitializing data...");
                    } // if
                    break;
                default:
                    if (this.ShowLogging)
                    {
                        Log.Warn("AT command ignored!");
                    } // if
                    retData = ObdBase.ElmOkPrompt;
                    break;
            } // switch

            return retData;
        } // ProcessAtCommand()

        /// <summary>
        /// Handles the echo.
        /// </summary>
        /// <param name="mode">if set to <c>true</c> echo on.</param>
        protected void HandleEcho(bool mode)
        {
            this.EchoOn = mode;
        } // HandleEcho()

        /// <summary>
        /// Handles the line feed.
        /// </summary>
        /// <param name="mode">if set to <c>true</c> use line feed.</param>
        protected void HandleLineFeed(bool mode)
        {
            this.LineFeed = mode;
        } // HandleLineFeed()

        /// <summary>
        /// Processes OBD mode 1 ('show current data') commands.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="data">The data.</param>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode1Command(int pid, string data)
        {
            var retData = this.mode1Results.ContainsKey(pid)
                ? this.mode1Results[pid] : ObdBase.ElmNoDataPrompt;

            return retData;
        } // ProcessMode1Command()

        /// <summary>
        /// Processes OBD mode 2 ('Show freeze frame data') commands.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="data">The data.</param>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode2Command(int pid, string data)
        {
            return ObdBase.ElmNoDataPrompt;
        } // ProcessMode2Command()

        /// <summary>
        /// Processes OBD mode 3 ('Show stored Diagnostic Trouble Codes) commands.
        /// </summary>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode3Command()
        {
            return this.Mode3Answer;
        } // ProcessMode3Command()

        /// <summary>
        /// Processes OBD mode 4 ('Clear Diagnostic Trouble Codes and stored values') commands.
        /// </summary>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode4Command()
        {
            Log.Warn("All DTC codes reset!");

            return "44 00";
        } // ProcessMode4Command()

        /// <summary>
        /// Processes OBD mode 5 ('Test results, oxygen sensor monitoring') commands.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="data">The data.</param>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode5Command(int pid, string data)
        {
            var retData = this.mode5Results.ContainsKey(pid)
                ? this.mode5Results[pid] : ElmNoDataPrompt;

            return retData;
        } // ProcessMode5Command()

        /// <summary>
        /// Processes OBD mode 6 ('Test results, other component/system monitoring') commands.
        /// </summary>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode6Command()
        {
            return ObdBase.ElmNoDataPrompt;
        } // ProcessMode6Command()

        /// <summary>
        /// Processes OBD mode 7 ('Show pending Diagnostic Trouble Codes') commands.
        /// </summary>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode7Command()
        {
            return this.Mode7Answer;
        } // ProcessMode7Command()

        /// <summary>
        /// Processes OBD mode 8 ('Control operation of on-board component/system') commands.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="data">The data.</param>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode8Command(int pid, string data)
        {
            return ObdBase.ElmNoDataPrompt;
        } // ProcessMode8Command()

        /// <summary>
        /// Processes OBD mode 9 ('Request vehicle information') commands.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="data">The data.</param>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode9Command(int pid, string data)
        {
            var retData = this.mode9Results.ContainsKey(pid)
                ? this.mode9Results[pid] : ElmNoDataPrompt;

            return retData;
        } // ProcessMode9Command()

        /// <summary>
        /// Processes OBD mode 10 ('Permanent Diagnostic Trouble Codes') commands.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="data">The data.</param>
        /// <returns>The answer string.</returns>
        protected virtual string ProcessMode10Command(int pid, string data)
        {
            return ObdBase.ElmNoDataPrompt;
        } // ProcessMode10Command()

        #endregion // PROTECTED METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Initializes the (simulator) results.
        /// </summary>
        private void InitializeResults()
        {
            this.mode1Results = new Dictionary<int, string>(50);
            this.mode5Results = new Dictionary<int, string>(10);
            this.mode6Results = new Dictionary<int, string>(10);
            this.mode9Results = new Dictionary<int, string>(10);

            // PIDs supported [01 - 20] => all PIDs
            this.mode1Results.Add(0x00, "41 00 FF FF FF FF");

            // Monitor status since DTCs cleared.
            this.mode1Results.Add(0x01, "41 01 00 00 00 00");

            // Engine coolant temperature
            this.mode1Results.Add(0x05, "41 05 90");

            // Engine RPM
            this.mode1Results.Add(0x0C, "41 0C 05 01");

            // Vehicle speed 
            this.mode1Results.Add(0x0D, "41 0D 50");

            // Intake air temperature
            this.mode1Results.Add(0x0F, "41 0F 50");

            // Throttle position
            this.mode1Results.Add(0X11, "4 11 60");

            // Engine oil temperature
            this.mode1Results.Add(0x5C, "41 5C 80");

            this.mode1Results.Add(0x5E, "41 5E 80");
        } // InitializeResults()

        /// <summary>
        /// Initializes results with Audi A4 values.
        /// </summary>
        private void InitializeResultsAudiA4()
        {
            this.mode1Results = new Dictionary<int, string>(50);
            this.mode5Results = new Dictionary<int, string>(10);
            this.mode6Results = new Dictionary<int, string>(10);
            this.mode9Results = new Dictionary<int, string>(10);

            // PIDs supported [01 - 20]
            this.mode1Results.Add(0x00, "41 00 BE 1F A8 13");
            //// PIDs: 0x01, 0x03, 0x04, 0x05, 0x06, 0x07, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x13, 0x15, 0x1C, 0x1F, 0x20

            // Monitor status since DTCs cleared.
            this.mode1Results.Add(0x01, "41010007E500");

            this.mode1Results.Add(0x02, "41020102NODATA");

            // Fuel system status: 
            this.mode1Results.Add(0x03, "41030200");

            this.mode1Results.Add(0x04, "410441");

            // Engine coolant temperature
            this.mode1Results.Add(0x05, "41 05 79");

            // Short term fuel % trim—Bank 1
            this.mode1Results.Add(0x06, "41 06 7F");

            // Long term fuel % trim—Bank 1
            this.mode1Results.Add(0x07, "41 07 7A");

            this.mode1Results.Add(0x08, "41 08 0108NODATA");

            this.mode1Results.Add(0x09, "41 09 0109NODATA");

            this.mode1Results.Add(0x0A, "41 0A 010ANODATA");

            // Intake manifold absolute pressure
            this.mode1Results.Add(0x0B, "41 0B 63");

            // Engine RPM:
            this.mode1Results.Add(0x0C, "41 0C 0C9A");

            // Vehicle speed: (motor not running)
            // this.mode1Results.Add(0x0D, "41 0D 00");
            // Vehicle speed: 80 = 128 km/hs
            this.mode1Results.Add(0x0D, "41 0D 80");

            // Timing advance
            this.mode1Results.Add(0x0E, "41 0E 6F");

            // Intake air temperature
            this.mode1Results.Add(0x0F, "41 0F 3B");

            this.mode1Results.Add(0x10, "41 10 0123");

            // Throttle position
            this.mode1Results.Add(0x11, "41 11 1E");

            // Oxygen sensors present
            this.mode1Results.Add(0x13, "41 13 03");

            // Bank 1, Sensor 1: Oxygen sensor voltage, Short term fuel trim
            this.mode1Results.Add(0x14, "41 14 0114NODATA");

            // Bank 1, Sensor 2: Oxygen sensor voltage, Short term fuel trim
            this.mode1Results.Add(0x15, "41 15 8A FF");

            // OBD standards this vehicle conforms to
            this.mode1Results.Add(0x1C, "41 1C 06");

            // Run time since engine start
            this.mode1Results.Add(0x1F, "41 1F 03D7");

            // PIDs supported [21 - 40]
            this.mode1Results.Add(0x20, "41 20 A0 05 B0 11");

            // Distance traveled with malfunction indicator lamp (MIL) on
            this.mode1Results.Add(0x21, "41 21 00 00");

            // Fuel Rail Pressure (diesel, or gasoline direct inject)
            this.mode1Results.Add(0x23, "41 23 0183");

            // Commanded evaporative purge
            this.mode1Results.Add(0x2E, "41 2E 4F");

            // Fuel Level Input
            this.mode1Results.Add(0x2F, "41 2F 012FNODATA");

            // # of warm-ups since codes cleared
            this.mode1Results.Add(0x30, "41 30 26");

            // Distance traveled since codes cleared
            this.mode1Results.Add(0x31, "41 31 00 F5");

            // Barometric pressure
            this.mode1Results.Add(0x33, "41 33 65");

            this.mode1Results.Add(0x34, "41 34 81188003");

            // Catalyst Temperature Bank 1, Sensor 1
            this.mode1Results.Add(0x3C, "41 3C 1279");

            // PIDs supported [41 - 60]
            this.mode1Results.Add(0x40, "41 40 FE D0 04 00");
            //// PIDs: 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4C, 0x56

            // Monitor status this drive cycle
            this.mode1Results.Add(0x41, "41 41 00 07 E1 01");

            // Control module voltage
            this.mode1Results.Add(0x42, "41 42 386A");

            // Absolute load value
            this.mode1Results.Add(0x43, "41 43 003C");

            // Fuel/Air commanded equivalence ratio
            this.mode1Results.Add(0x44, "41 44 8000");

            // Relative throttle position
            this.mode1Results.Add(0x45, "41 45 05");

            // Ambient air temperature
            this.mode1Results.Add(0x46, "41 46 2F");

            // Absolute throttle position B
            this.mode1Results.Add(0x47, "41 47 1E");

            // Accelerator pedal position D
            this.mode1Results.Add(0x49, "41 49 26");

            // Accelerator pedal position E
            this.mode1Results.Add(0x4A, "41 4A 26");

            // Commanded throttle actuator
            this.mode1Results.Add(0x4C, "41 4C 06");

            // Long term secondary oxygen sensor trim bank 1 and bank 3
            this.mode1Results.Add(0x56, "41 56 80 ");

            this.Mode3Answer = "430000";

            // No mode 5 PIDs

            // this.Mode6Answer = "4600C0000001";

            // No mode 9 PIDs
            this.mode9Results.Add(0x00, "490054400000");
            this.mode9Results.Add(0x02, "014 \r\n0: 49 02 01 57 41 55 \r\n1: 5A 5A 5A 38 4B 32 39 \r\n2: 41 30 31 31 32 32 33 ");

            // ??? 09,04 ???
            this.mode9Results.Add(0x04, "013 \r\n0: 49 04 01 38 4B 31 \r\n1: 39 30 37 31 31 35 4A \r\n2: 20 20 30 30 30 31 00");
            this.mode9Results.Add(0x0A, "017 \r\n0: 49 0A 01 45 43 4D \r\n1: 00 2D 45 6E 67 69 6E \r\n2: 65 43 6F 6E 74 72 6F \r\n3: 6C 00 00 00 00 00 00 ");
        } // InitializeResultsAudiA4()

        /// <summary>
        /// Initializes results with Audi A3 values.
        /// </summary>
        private void InitializeResultsAudiA3()
        {
            this.mode1Results = new Dictionary<int, string>(50);
            this.mode5Results = new Dictionary<int, string>(10);
            this.mode6Results = new Dictionary<int, string>(10);
            this.mode9Results = new Dictionary<int, string>(10);

            // PIDs supported [01 - 20]
            this.mode1Results.Add(0x00, "41 00 BE 3E B8 13");
            //// PIDs: 0x01, 0x03, 0x04, 0x05, 0x06, 0x07, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x11, 0x13, 0x14, 0x15, 0x1C, 0x1F, 0x20

            // Monitor status since DTCs cleared.
            this.mode1Results.Add(0x01, "41 01 01 87 E1 00");
            this.Mode3Answer = "43 01 33 00 00 00 00"; // = P0133 = 'oxygen sensor circuit slow response'
            //// this.Mode3Answer = "43 01 33 02 44 23 11\r\n81 34 B1 35 D1 36";

            // Fuel system status: 
            this.mode1Results.Add(0x03, "41 03 00 00");

            // Calculated engine load value: (motor not running)
            this.mode1Results.Add(0x04, "41 04 00");

            // Engine coolant temperature
            this.mode1Results.Add(0x05, "41 05 79");

            // Short term fuel % trim—Bank 1
            this.mode1Results.Add(0x06, "41 06 80");

            // Long term fuel % trim—Bank 1
            this.mode1Results.Add(0x07, "41 07 81");

            // Intake manifold absolute pressure
            this.mode1Results.Add(0x0B, "41 0B 63");

            // Engine RPM: (motor not running)
            // this.mode1Results.Add(0x0C, "41 0C 00 00");
            // Engine RPM: (motor not running)
            this.mode1Results.Add(0x0C, "41 0C 10 00");

            // Vehicle speed: (motor not running)
            // this.mode1Results.Add(0x0D, "41 0D 00");
            // Vehicle speed: 80 = 128 km/hs
            this.mode1Results.Add(0x0D, "41 0D 80");

            // Timing advance
            this.mode1Results.Add(0x0E, "41 0E 80");

            // Intake air temperature
            this.mode1Results.Add(0x0F, "41 0F 4F");

            // Throttle position
            this.mode1Results.Add(0x11, "41 11 2B");

            // Oxygen sensors present
            this.mode1Results.Add(0x13, "41 13 03");

            // Bank 1, Sensor 1: Oxygen sensor voltage, Short term fuel trim
            this.mode1Results.Add(0x14, "41 14 5A 80");

            // Bank 1, Sensor 2: Oxygen sensor voltage, Short term fuel trim
            this.mode1Results.Add(0x15, "41 15 5A FF");

            // OBD standards this vehicle conforms to
            this.mode1Results.Add(0x1C, "41 1C 06");

            // Run time since engine start
            this.mode1Results.Add(0x1F, "41 1F 00 00");

            // PIDs supported [21 - 40]
            this.mode1Results.Add(0x20, "41 20 A0 07 A0 11");
            //// PIDs 0x21, 0x23, 0x2E, 0x2F, 0x30, 0x31, 0x33, 0x3C, 0x40ss

            // Distance traveled with malfunction indicator lamp (MIL) on
            this.mode1Results.Add(0x21, "41 21 00 00");

            // Fuel Rail Pressure (diesel, or gasoline direct inject)
            this.mode1Results.Add(0x23, "41 23 08 82");

            // Commanded evaporative purge
            this.mode1Results.Add(0x2E, "41 2E 00");

            // Fuel Level Input
            this.mode1Results.Add(0x2F, "41 2F 7F");

            // # of warm-ups since codes cleared
            this.mode1Results.Add(0x30, "41 30 FD");

            // Distance traveled since codes cleared
            this.mode1Results.Add(0x31, "41 31 18 FF");

            // Barometric pressure
            this.mode1Results.Add(0x33, "41 33 63");

            // Catalyst Temperature Bank 1, Sensor 1
            this.mode1Results.Add(0x3C, "41 3C 01 BD");

            // PIDs supported [41 - 60]
            this.mode1Results.Add(0x40, "41 40 FE D0 04 00");
            //// PIDs 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4C, 0x56

            // Monitor status this drive cycle
            this.mode1Results.Add(0x41, "41 41 00 07 E1 01");

            // Control module voltage
            this.mode1Results.Add(0x42, "41 42 2E EF");

            // Absolute load value
            this.mode1Results.Add(0x43, "41 43 00 00");

            // Fuel/Air commanded equivalence ratio
            this.mode1Results.Add(0x44, "41 44 FF FF");

            // Relative throttle position
            this.mode1Results.Add(0x45, "41 45 11");

            // Ambient air temperature
            this.mode1Results.Add(0x46, "41 46 2C");

            // Absolute throttle position B
            this.mode1Results.Add(0x47, "41 47 2A");

            // Accelerator pedal position D
            this.mode1Results.Add(0x49, "41 49 24");

            // Accelerator pedal position E
            this.mode1Results.Add(0x4A, "41 4A 25 ");

            // Commanded throttle actuator
            this.mode1Results.Add(0x4C, "41 4C 04 ");

            // Long term secondary oxygen sensor trim bank 1 and bank 3
            this.mode1Results.Add(0x56, "41 56 80 ");

            this.mode6Results.Add(0x00, "46 00 C0 00 00 01");

            this.Mode7Answer = "47 00";

            this.mode9Results.Add(0x00, "49 00 54 40 00 00");
            //// PIDs: 0x02, 0x04, 0x06, 0x0A

            // CAN format
            this.mode9Results.Add(0x02, "014\r\n0: 49 02 01 31 44 34\r\n1: 47 50 30 30 52 35 35\r\n2: 42 31 32 33 34 35 36");

            this.mode9Results.Add(0x06, "01 79 9C 08 CF");

            this.mode9Results.Add(0x0A, "017\r\n0: 49 0A 01 45 43 4D\r\n1: 00 2D 45 6E 67 69 6E\r\n2: 65 43 6F 6E 74 72 6F\r\n3: 6C 00 00 00 00 00 00");
            //// = "ECM-EngineControl
        } // InitializeResultsAudiA3()

        /// <summary>
        /// Initializes results with Volkswagen Golf values.
        /// </summary>
        private void InitializeResultsVwGolf()
        {
            this.mode1Results = new Dictionary<int, string>(50);
            this.mode5Results = new Dictionary<int, string>(10);
            this.mode6Results = new Dictionary<int, string>(10);
            this.mode9Results = new Dictionary<int, string>(10);

            // PIDs supported [01 - 20]
            this.mode1Results.Add(0x00, "41 00 BE 3E B8 13");
            //// PIDs: 0x01, 0x03, 0x04, 0x05, 0x06, 0x07, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x11, 0x13, 0x14, 0x15, 0x1C, 0x1F, 0x20

            // Monitor status since DTCs cleared.
            this.mode1Results.Add(0x01, "41 01 01 07 E1 00");
            this.Mode3Answer = "43 00";
            
            // Fuel system status: 
            this.mode1Results.Add(0x03, "41 03 02 00");

            // Calculated engine load value
            this.mode1Results.Add(0x04, "41 04 39");

            // Engine coolant temperature
            this.mode1Results.Add(0x05, "41 05 7C");

            // Short term fuel % trim—Bank 1
            this.mode1Results.Add(0x06, "41 06 80");

            // Long term fuel % trim—Bank 1
            this.mode1Results.Add(0x07, "41 07 83");

            // Intake manifold absolute pressure
            this.mode1Results.Add(0x0B, "41 0B 22");

            // Engine RPM
            this.mode1Results.Add(0x0C, "41 0C 0A 34");

            // Vehicle speed: 80 = 128 km/hs
            this.mode1Results.Add(0x0D, "41 0D 80");

            // Timing advance
            this.mode1Results.Add(0x0E, "41 0E 75");

            // Intake air temperature
            this.mode1Results.Add(0x0F, "41 0F 4B");

            // Throttle position
            this.mode1Results.Add(0x11, "41 11 1F");

            // Oxygen sensors present
            this.mode1Results.Add(0x13, "41 13 03");

            // Bank 1, Sensor 1: Oxygen sensor voltage, Short term fuel trim
            this.mode1Results.Add(0x14, "41 14 9E 80");

            // Bank 1, Sensor 2: Oxygen sensor voltage, Short term fuel trim
            this.mode1Results.Add(0x15, "41 15 50 FF");

            // OBD standards this vehicle conforms to
            this.mode1Results.Add(0x1C, "41 1C 06");

            // Run time since engine start
            this.mode1Results.Add(0x1F, "41 1F 00 53");

            // PIDs supported [21 - 40]
            this.mode1Results.Add(0x20, "41 20 A0 05 A0 11");

            // Distance traveled with malfunction indicator lamp (MIL) on
            this.mode1Results.Add(0x21, "41 21 00 00");

            // Fuel Rail Pressure (diesel, or gasoline direct inject)
            this.mode1Results.Add(0x23, "41 23 06 60");

            // Commanded evaporative purge
            this.mode1Results.Add(0x2E, "41 2E 00");

            // # of warm-ups since codes cleared
            this.mode1Results.Add(0x30, "41 30 A7");

            // Distance traveled since codes cleared
            this.mode1Results.Add(0x31, "41 31 0A 59");

            // Barometric pressure
            this.mode1Results.Add(0x33, "41 33 63");

            // Catalyst Temperature Bank 1, Sensor 1
            this.mode1Results.Add(0x3C, "41 3C 09 75");

            // PIDs supported [41 - 60]
            this.mode1Results.Add(0x40, "41 40 FE D0 04 00");
            //// PIDs 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4C, 0x56

            // Monitor status this drive cycle
            this.mode1Results.Add(0x41, "41 41 00 07 E1 01");

            // Control module voltage
            this.mode1Results.Add(0x42, "41 42 39 CB");

            // Absolute load value
            this.mode1Results.Add(0x43, "41 43 00 34");

            // Fuel/Air commanded equivalence ratio
            this.mode1Results.Add(0x44, "41 44 80 00");

            // Relative throttle position
            this.mode1Results.Add(0x45, "41 45 06");

            // Ambient air temperature
            this.mode1Results.Add(0x46, "41 46 32");

            // Absolute throttle position B
            this.mode1Results.Add(0x47, "41 47 1F");

            // Accelerator pedal position D
            this.mode1Results.Add(0x49, "41 49 27");

            // Accelerator pedal position E
            this.mode1Results.Add(0x4A, "41 4A 26");

            // Commanded throttle actuator
            this.mode1Results.Add(0x4C, "41 4C 07");

            // Long term secondary oxygen sensor trim bank 1 and bank 3
            this.mode1Results.Add(0x56, "41 56 80");

            this.mode6Results.Add(0x00, "46 00 C0 00 00 01");

            this.Mode7Answer = "NO DATA";

            // Mode 9 supported PIDs (01 to 20)
            this.mode9Results.Add(0x00, "49 00 55 40 00 00");

            // Vehicle Identification Number 
            this.mode9Results.Add(0x02, "014\r\n0: 49 02 01 57 56 57\r\n1: 5A 5A 5A 41 55 5A 45\r\n2: 57 30 33 33 33 33 33");

            // Calibration ID
            this.mode9Results.Add(0x04, "013\r\n0: 49 04 01 30 34 45\r\n1: 30 31 36 45 20 35 35\r\n2: 34 32 42 44 41 44 00");

            // Calibration Verification Numbers (CVN)
            this.mode9Results.Add(0x06, "49 06 01 3D AB E9 BC");

            // In-use performance tracking for spark ignition vehicles
            this.mode9Results.Add(0x08, "02B\r\n0: 49 08 14 01 8D 06\r\n1: 8C 00 6A 01 8D 00 00\r\n2: 00 00 00 43 01 8D 00\r\n3: 00 00 00 05 42 01 8D\r\n4: 00 00 00 00 00 00 00\r\n5: 00 01 27 01 8D 00 00\r\n6: 00 00 00 00 00 00 00 ");

            // ECU name
            this.mode9Results.Add(0x0A, "017\r\n0: 49 0A 01 45 43 4D\r\n1: 00 2D 45 6E 67 69 6E\r\n2: 65 43 6F 6E 74 72 6F\r\n3: 6C 00 00 00 00 00 00 ");
            //// = ECM-EngineControl
        } // InitializeResultsVwGolf()
        #endregion // PRIVATE METHODS
    } // ObdSimulator
} // TgSoft.OBD.ObdSimulator
