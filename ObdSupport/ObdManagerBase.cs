#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdManagerBase.cs" company="Tethys">
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

/*****************************************************************************
 * INFO
 * ----
 * 
 * For OBD-II PID detail, see
 * - http://en.wikipedia.org/wiki/OBD-II_PIDs
 * 
 * For DTC codes see
 * - http://www.totalcardiagnostics.com/support/Knowledgebase/Article/View/21/0/genericmanufacturer-obd2-codes-and-their-meanings
 * 
 ****************************************************************************/

namespace Tethys.OBD
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Tethys.Logging;

    /// <summary>
    /// Base class for OBD client applications.
    /// </summary>
    public abstract class ObdManagerBase : ObdBase
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Logger for this class.
        /// </summary>
        private static readonly ILog Log
            = LogManager.GetLogger(typeof(ObdManagerBase));
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// The read timeout (1000 milliseconds).
        /// </summary>
        public const int ReadTimeout = 1000;

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public IObdDeviceConnection Connection { get; private set; }

        /// <summary>
        /// Occurs when an error has been encountered.
        /// </summary>
        public event EventHandler<string> ErrorEncountered;
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ObdManagerBase" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected ObdManagerBase(IObdDeviceConnection connection)
        {
            this.Connection = connection;
        } // ObdManagerBase()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Resets this instance.
        /// </summary>
        public virtual void Reset()
        {
        } // Reset()

        /// <summary>
        /// Initializes the connection.
        /// </summary>
        /// <returns>Any text data returned by the device.</returns>
        public virtual async Task<string> InitializeConnection()
        {
            await this.Connection.WriteToDevice("ATZ\r\n"); // reset all
            await this.Connection.WriteToDevice("ATE0\r\n"); // echo off
            // await this.Connection.WriteToDevice("ATH0\r\n"); // headers off
            await this.Connection.WriteToDevice("ATL0\r\n"); // linefeed off
            // await this.Connection.WriteToDevice("ATS0\r\n"); // printing of spaces off
            await this.Connection.WriteToDevice("ATSP00\r\n"); // Set Protocol to Auto and save it

            // read all available data from connection
            return await this.Connection.ReadFromDevice();
        } // InitializeConnection()

        /// <summary>
        /// Clears the buffer.
        /// </summary>
        /// <returns>Any text data returned by the device.</returns>
        public virtual async Task<string> ClearBuffer()
        {
            // read all available data from connection
            return await this.Connection.ReadFromDevice();
        } // ClearBuffer()

        /// <summary>
        /// Resets the connection.
        /// </summary>
        /// <returns>Any text data returned by the device.</returns>
        public virtual async Task<string> ResetConnection()
        {
            await this.Connection.WriteToDevice("ATZ\r\n");

            // read all available data from connection
            return await this.Connection.ReadFromDevice();
        } // ResetConnection()

        /// <summary>
        /// Sends the raw command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns>The OBD reply text.</returns>
        public async Task<string> SendRawCommand(string cmd)
        {
            await this.Connection.WriteToDevice(cmd + "\r\n");
            
            // read all available data from connection
            return await this.Connection.ReadFromDevice(ObdBase.ElmPrompt);
        } // SendRawCommand()

        /// <summary>
        /// Sends the raw command.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="pid">The pid.</param>
        /// <returns>
        /// The OBD reply text.
        /// </returns>
        public async Task<string> SendRawPidCommand(int mode, int pid)
        {
            if (mode == 5)
            {
                await this.Connection.WriteToDevice(string.Format("{0:X2} {1:X4}\r\n", mode, pid));
            }
            else
            {
                await this.Connection.WriteToDevice(string.Format("{0:X2} {1:X2}\r\n", mode, pid));
            } // if

            return await this.Connection.ReadFromDevice();
        } // SendRawPidCommand()

        /// <summary>
        /// Sends PID command.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="rawResult">if set to <c>true</c> return the 
        /// unprocessed result.</param>
        /// <returns>
        /// The data part of the OBD reply.
        /// </returns>
        public async Task<string> SendPidCommand(int mode, int pid, bool rawResult = false)
        {
            string answer;
            if (pid >= 0)
            {
                var format = "{0:X2}{1:X2}";
                if (pid > 0xff)
                {
                    format = "{0:X2}{1:X4}";
                } // if
                answer = string.Format(format, mode + 0x40, pid);
                format += "\r\n";
                await this.Connection.WriteToDevice(string.Format(format, mode, pid));
            }
            else
            {
                var format = "{0:X2}";
                answer = string.Format(format, mode + 0x40);
                format += "\r\n";
                await this.Connection.WriteToDevice(string.Format(format, mode));
            } // if

            var text = await this.Connection.ReadFromDevice(ElmPrompt);

            text = text.Trim();
            text = ObdSupport.GetLastAnswer(text);
            text = text.Replace(ObdBase.ElmPrompt, string.Empty);
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\n", string.Empty);

            if (rawResult)
            {
                return text;
            } // if

            text = text.Replace(" ", string.Empty);

            var index = text.IndexOf(answer, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                // not an answer four our command or
                // something like '?' or "NO DATA"
                return string.Empty;
            } // if

            text = text.Substring(index + answer.Length);

            return text;
        } // SendPidCommand()

        /// <summary>
        /// Sends a PID command that should result in a multi-line answer.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="pid">The pid.</param>
        /// <returns>
        /// The data part of the OBD reply.
        /// </returns>
        public async Task<string> SendPidMultiLineCommand(int mode, int pid)
        {
            string answer;
            if (pid >= 0)
            {
                var format = "{0:X2}{1:X2}";
                if (pid > 0xff)
                {
                    format = "{0:X2}{1:X4}";
                } // if
                answer = string.Format(format, mode + 0x40, pid);
                format += "\r\n";
                await this.Connection.WriteToDevice(string.Format(format, mode, pid));
            }
            else
            {
                var format = "{0:X2}";
                answer = string.Format(format, mode + 0x40);
                format += "\r\n";
                await this.Connection.WriteToDevice(string.Format(format, mode));
            } // if

            var text = await this.Connection.ReadFromDevice(ElmPrompt);
            text = text.Trim();
          
            int length;
            var index = 0;
            while ((index < text.Length) && (char.IsDigit(text[index])))
            {
                index++;
            } // while

            var help = text.Substring(0, index);
            try
            {
                length = int.Parse(help, NumberStyles.HexNumber);
            }
            catch
            {
                // invalid or unknown format
                return string.Empty;
            } // if

            text = text.Substring(index);
            text = text.Replace(ObdBase.ElmPrompt, string.Empty);
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\n", string.Empty);
            text = text.Replace(" ", string.Empty);
            return ObdSupport.GetMultilineCanString(text, answer, length);
        } // SendPidMultiLineCommand()
        
        /// <summary>
        /// Gets the battery voltage.
        /// </summary>
        /// <returns>The battery voltage.</returns>
        public async Task<float> GetBatteryVoltage()
        {
            await this.Connection.WriteToDevice("ATRV\r\n");

            var retText = await this.Connection.ReadFromDevice();
            
            // echo might be on ==> remove it
            retText = retText.Replace("ATRV", string.Empty);
            retText = retText.Replace("\r\n", string.Empty);

            var index = retText.IndexOf('V');
            if (index < 0)
            {
                return 0.0F;
            } // if

            retText = retText.Substring(0, index);

            var retVal = 0.0F;
            try
            {
                var cultEn = new CultureInfo("en-US");
                retVal = float.Parse(retText, NumberStyles.Float, cultEn);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(CultureInfo.CurrentCulture,
                    "Error parsing battery voltage ('{0}')", ex, retText);
            } // catch

            return retVal;
        } // GetBatteryVoltage()

        /// <summary>
        /// Gets the current ELM327 protocol.
        /// </summary>
        /// <returns>The current ELM327 protocol string.</returns>
        public async Task<string> GetCurrentProtocol()
        {
            await this.Connection.WriteToDevice("ATDP\r\n");

            var text = await this.Connection.ReadFromDevice();

            // echo might be on ==> remove it
            text = text.Replace("ATDP", string.Empty);
            text = text.Replace("\r\n", string.Empty);
            text = text.Replace(">", string.Empty);
            text = text.Trim();

            return text;
        } // GetCurrentProtocol()

        /// <summary>
        /// Gets the name of the OBD device.
        /// </summary>
        /// <returns>The name of the OBD device.</returns>
        public async Task<string> GetObdDeviceName()
        {
            const string Cmd = "ATI";

            await this.Connection.WriteToDevice("ATI\r\n");

            var text = await this.Connection.ReadFromDevice();

            if (text.StartsWith(Cmd))
            {
                text = text.Substring(Cmd.Length);
            } // if

            text = text.Replace(">", string.Empty);
            text = text.Trim();

            return text;
        } // GetObdDeviceName()

        /// <summary>
        /// Gets the supported Mode 1 PIDs 0 .. 20.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedPids20()
        {
            var text = await this.SendPidCommand(1, 0);
            return ObdSupport.HexToInt(text);
        } // GetSupportedPids20()

        /// <summary>
        /// Gets the monitor status.
        /// </summary>
        /// <returns>The monitor status.</returns>
        public async Task<uint> GetMonitorStatus()
        {
            var text = await this.SendPidCommand(1, 1);
            return ObdSupport.HexToInt(text);
        } // GetMonitorStatus()

        /// <summary>
        /// Gets the fuel system status.
        /// </summary>
        /// <returns>The fuel system status.</returns>
        public async Task<uint> GetFuelSystemStatus()
        {
            var text = await this.SendPidCommand(1, 3);
            return ObdSupport.HexToInt(text);
        } // GetFuelSystemStatus()

        /// <summary>
        /// Gets the calculated engine load.
        /// </summary>
        /// <returns>The calculated engine load (0 .. 100).</returns>
        public async Task<int> GetCalculatedEngineLoad()
        {
            var text = await this.SendPidCommand(1, 4);
            return ObdSupport.GetCalculatedEngineLoad(text);
        } // GetCalculatedEngineLoad()

        /// <summary>
        /// Gets the engine coolant temperature.
        /// </summary>
        /// <returns>The engine coolant temperature (-40 .. 215).</returns>
        public async Task<int> GetEngineCoolantTemperature()
        {
            var text = await this.SendPidCommand(1, 5);
            return ObdSupport.GetEngineCoolantTemperature(text);
        } // GetEngineCoolantTemperature()

        /// <summary>
        /// Gets the short term fuel trim.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <returns>The short term fuel trim value.</returns>
        public async Task<float> GetShortTermFuelTrim(int bank)
        {
            var pid = 6;
            if (bank == 2)
            {
                pid = 8;
            } // if
            var text = await this.SendPidCommand(1, pid);
            return ObdSupport.GetFuelTrimValue(text);
        } // GetShortTermFuelTrim()

        /// <summary>
        /// Gets the long term fuel trim.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <returns>The long term fuel trim value.</returns>
        public async Task<float> GetLongTermFuelTrim(int bank)
        {
            var pid = 7;
            if (bank == 2)
            {
                pid = 9;
            } // if
            var text = await this.SendPidCommand(1, pid);
            return ObdSupport.GetFuelTrimValue(text);
        } // GetLongTermFuelTrim()

        /// <summary>
        /// Gets the fuel pressure.
        /// </summary>
        /// <returns>The fuel pressure.</returns>
        public async Task<int> GetFuelPressure()
        {
            var text = await this.SendPidCommand(1, 0x0A);
            return (int)(ObdSupport.HexToInt(text) * 3);
        } // GetFuelPressure()

        /// <summary>
        /// Gets the intake manifold absolute pressure.
        /// </summary>
        /// <returns>
        /// The intake manifold absolute pressure.
        /// </returns>
        public async Task<byte> GetIntakeManifoldAbsolutePressure()
        {
            var text = await this.SendPidCommand(1, 0x0B);
            return (byte)(ObdSupport.HexToInt(text));
        } // GetIntakeManifoldAbsolutePressure()

        /// <summary>
        /// Gets the engine RPM.
        /// </summary>
        /// <returns>The engine RPM.</returns>
        public async Task<int> GetEngineRpm()
        {
            var text = await this.SendPidCommand(1, 0x0C);
            return ObdSupport.GetEngineRpm(text);
        } // GetEngineRpm()

        /// <summary>
        /// Gets the vehicle speed.
        /// </summary>
        /// <returns>The vehicle speed (0 .. 255).</returns>
        public async Task<int> GetVehicleSpeed()
        {
            var text = await this.SendPidCommand(1, 0x0D);
            return ObdSupport.GetVehicleSpeed(text);
        } // GetVehicleSpeed()

        /// <summary>
        /// Gets the timing advance.
        /// </summary>
        /// <returns>The timing advance.</returns>
        public async Task<float> GetTimingAdvance()
        {
            var text = await this.SendPidCommand(1, 0x0E);
            return ((float)ObdSupport.HexToInt(text) - 128) / 2;
        } // GetTimingAdvance()

        /// <summary>
        /// Gets the intake air temperature.
        /// </summary>
        /// <returns>The intake air temperature.</returns>
        public async Task<int> GetIntakeAirTemperature()
        {
            var text = await this.SendPidCommand(1, 0x0F);
            return (int)(ObdSupport.HexToInt(text) - 40);
        } // GetIntakeAirTemperature()

        /// <summary>
        /// Gets the MAF air flow rate.
        /// </summary>
        /// <returns>The MAF air flow rate.</returns>
        public async Task<float> GetMafAirFlowRate()
        {
            var text = await this.SendPidCommand(1, 0x10);
            return ObdSupport.GetMafAirFlowRate(text);
        } // GetMafAirFlowRate()

        /// <summary>
        /// Gets the throttle position.
        /// </summary>
        /// <returns>The throttle position.</returns>
        public async Task<float> GetThrottlePosition()
        {
            var text = await this.SendPidCommand(1, 0x11);
            return ObdSupport.GetThrottlePosition(text);
        } // GetThrottlePosition()

        /// <summary>
        /// Gets the oxygen sensors present as bit code value.
        /// </summary>
        /// <returns>Oxygen sensors present as bit code value.</returns>
        public async Task<byte> GetOxygenSensorsPresent()
        {
            var text = await this.SendPidCommand(1, 0x13);
            return (byte)ObdSupport.HexToInt(text);
        } // GetOxygenSensorsPresent()

        /// <summary>
        /// Gets the oxygen sensor voltage.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="sensor">The sensor.</param>
        /// <returns>The oxygen sensor voltage.</returns>
        public async Task<float> GetOxygenSensorVoltage(int bank, int sensor)
        {
            var pid = ObdSupport.GetOxygenSensorPid(bank, sensor);
            var text = await this.SendPidCommand(1, pid);
            var val = (short)ObdSupport.HexToInt(text);
            var voltage = ((float)(val >> 8)) / 200;
            return voltage;
        } // GetOxygenSensorVoltage()

        /// <summary>
        /// Gets the oxygen sensor short term fuel trim.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="sensor">The sensor.</param>
        /// <returns>The oxygen sensor short term fuel trim.</returns>
        public async Task<float> GetOxygenSensorFuelTrim(int bank, int sensor)
        {
            var pid = ObdSupport.GetOxygenSensorPid(bank, sensor);
            var text = await this.SendPidCommand(1, pid);
            if (text.Length < 4)
            {
                return -200;
            } // if
            text = text.Substring(2);
            if (text == "FF")
            {
                return -200;
            } // if

            var val = ObdSupport.GetFuelTrimValue(text);
            return val;
        } // GetOxygenSensorFuelTrim()

        /// <summary>
        /// Gets the OBD standards this vehicle conforms to.
        /// </summary>
        /// <returns>The OBD standards this vehicle conforms to.</returns>
        public async Task<byte> GetCarObdStandard()
        {
            var text = await this.SendPidCommand(1, 0x1C);
            return (byte)ObdSupport.HexToInt(text);
        } // GetCarObdStandards()

        /// <summary>
        /// Gets the runtime since engine start (in seconds).
        /// </summary>
        /// <returns>The runtime since engine start (in seconds).</returns>
        public async Task<uint> GetRuntimeSinceEngineStart()
        {
            var text = await this.SendPidCommand(1, 0x1F);
            return ObdSupport.HexToInt(text);
        } // GetRuntimeSinceEngineStart()

        /// <summary>
        /// Gets the supported Mode 1 PIDs 0x21 .. 0x40.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedPids40()
        {
            var text = await this.SendPidCommand(1, 0x20);
            return ObdSupport.HexToInt(text);
        } // GetSupportedPids40()

        /// <summary>
        /// Gets the supported Mode 1 PIDs 0x41 .. 0x60.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedPids60()
        {
            var text = await this.SendPidCommand(1, 0x40);
            return ObdSupport.HexToInt(text);
        } // GetSupportedPids60()

        /// <summary>
        /// Gets the supported Mode 1 PIDs 0x61 .. 0x80.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedPids80()
        {
            var text = await this.SendPidCommand(1, 0x60);
            return ObdSupport.HexToInt(text);
        } // GetSupportedPids80()

        /// <summary>
        /// Gets the supported Mode 1 PIDs 0x81 .. 0xA0.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedPidsA0()
        {
            var text = await this.SendPidCommand(1, 0x80);
            return ObdSupport.HexToInt(text);
        } // GetSupportedPidsA0()

        /// <summary>
        /// Gets the supported Mode 1 PIDs 0xA1 .. 0xC0.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedPidsC0()
        {
            var text = await this.SendPidCommand(1, 0xA0);
            return ObdSupport.HexToInt(text);
        } // GetSupportedPidsC0()

        /// <summary>
        /// Gets the supported Mode 1 PIDs 0xC1 .. 0xE0.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedPidsE0()
        {
            var text = await this.SendPidCommand(1, 0xC0);
            return ObdSupport.HexToInt(text);
        } // GetSupportedPidsE0()

        /// <summary>
        /// Gets the supported Mode 5 PIDs $01 – $20.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedMode5Pids()
        {
            var text = await this.SendPidCommand(5, 0x0100);
            return ObdSupport.HexToInt(text);
        } // GetSupportedMode5Pids()

        /// <summary>
        /// Gets the supported Mode 9 PIDs 01 – 20.
        /// </summary>
        /// <returns>Bit coded PIDs.</returns>
        public async Task<uint> GetSupportedMode9Pids()
        {
            var text = await this.SendPidCommand(9, 0x00);
            return ObdSupport.HexToInt(text);
        } // GetSupportedMode9Pids()

        /// <summary>
        /// Gets the distance traveled with malfunction indicator lamp (MIL) on.
        /// </summary>
        /// <returns>The distance traveled with malfunction indicator 
        /// lamp (MIL) on.</returns>
        public async Task<uint> GetDistanceTraveledWithMilOn()
        {
            var text = await this.SendPidCommand(1, 0x21);
            return ObdSupport.HexToInt(text);
        } // GetDistanceTraveledWithMilOn()

        /// <summary>
        /// Gets the fuel rail pressure.
        /// </summary>
        /// <returns>The fuel rail pressure.</returns>
        public async Task<uint> GetFuelRailPressure()
        {
            var text = await this.SendPidCommand(1, 0x23);
            return ObdSupport.HexToInt(text) * 10;
        } // GetFuelRailPressure()

        /// <summary>
        /// Gets the commanded evaporative purge.
        /// </summary>
        /// <returns>The commanded evaporative purge.</returns>
        public async Task<float> GetCommandedEvaporativePurge()
        {
            var text = await this.SendPidCommand(1, 0x2e);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetCommandedEvaporativePurge()

        /// <summary>
        /// Gets the fuel level input.
        /// </summary>
        /// <returns>The fuel level input.</returns>
        public async Task<float> GetFuelLevelInput()
        {
            var text = await this.SendPidCommand(1, 0x2f);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetFuelLevelInput()

        /// <summary>
        /// Gets the number of warm-ups since codes have been cleared.
        /// </summary>
        /// <returns>The number of warm-ups since codes have been cleared.</returns>
        public async Task<uint> GetWarmupsSinceCodesCleared()
        {
            var text = await this.SendPidCommand(1, 0x30);
            return ObdSupport.HexToInt(text);
        } // GetWarmupsSinceCodesCleared()

        /// <summary>
        /// Gets the distance traveled since codes have been cleared.
        /// </summary>
        /// <returns>The distance traveled since codes have been cleared.</returns>
        public async Task<uint> GetDistanceTraveledSinceCodesCleared()
        {
            var text = await this.SendPidCommand(1, 0x31);
            return ObdSupport.HexToInt(text);
        } // GetDistanceTraveledSinceCodesCleared()

        /// <summary>
        /// Gets the barometric pressure.
        /// </summary>
        /// <returns>The barometric pressure.</returns>
        public async Task<byte> GetBarometricPressure()
        {
            var text = await this.SendPidCommand(1, 0x33);
            return (byte)ObdSupport.HexToInt(text);
        } // GetBarometricPressure()

        /// <summary>
        /// Gets the catalyst temperature.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="sensor">The sensor.</param>
        /// <returns>The catalyst temperature.</returns>
        public async Task<float> GetCatalystTemperature(int bank, int sensor)
        {
            var pid = ObdSupport.GetCatalystTemperaturePid(bank, sensor);
            var text = await this.SendPidCommand(1, pid);
            return ((float)ObdSupport.HexToInt(text) / 10) - 40;
        } // GetCatalystTemperature()

        /// <summary>
        /// Gets the monitor status for this drive cycle.
        /// </summary>
        /// <returns>The monitor status for this drive cycle.</returns>
        public async Task<uint> GetMonitorStatusDriveCycle()
        {
            var text = await this.SendPidCommand(1, 0x41);
            return ObdSupport.HexToInt(text);
        } // GetMonitorStatusDriveCycle()

        /// <summary>
        /// Gets the control module voltage.
        /// </summary>
        /// <returns>The control module voltage.</returns>
        public async Task<float> GetControlModuleVoltage()
        {
            var text = await this.SendPidCommand(1, 0x42);
            return ObdSupport.HexToInt(text) / 1000;
        } // GetControlModuleVoltage()

        /// <summary>
        /// Gets the absolute load value.
        /// </summary>
        /// <returns>The absolute load value.</returns>
        public async Task<float> GetAbsoluteLoadValue()
        {
            var text = await this.SendPidCommand(1, 0x43);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetAbsoluteLoadValue()

        /// <summary>
        /// Gets the commanded fuel air equivalence ratio.
        /// </summary>
        /// <returns>The commanded fuel air equivalence ratio.</returns>
        public async Task<float> GetCommandedFuelAirRatio()
        {
            var text = await this.SendPidCommand(1, 0x44);
            return ((float)ObdSupport.HexToInt(text)) / 32768;
        } // GetCommandedFuelAirRatio()

        /// <summary>
        /// Gets the relative throttle position.
        /// 'Relative throttle position' means the 'learned' TP position.
        /// The ECU calculates the voltage offset for the closed-throttle
        /// position, and uses it as the 0%.
        /// </summary>
        /// <returns>The relative throttle position.</returns>
        public async Task<float> GetRelativeThrottlePosition()
        {
            var text = await this.SendPidCommand(1, 0x45);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetRelativeThrottlePosition()

        /// <summary>
        /// Gets the ambient air temperature.
        /// </summary>
        /// <returns>The ambient air temperature.</returns>
        public async Task<int> GetAmbientAirTemperature()
        {
            var text = await this.SendPidCommand(1, 0x46);
            return (int)ObdSupport.HexToInt(text) - 40;
        } // GetAmbientAirTemperature()

        /// <summary>
        /// Gets the absolute throttle position B.
        /// </summary>
        /// <returns>The absolute throttle position B.</returns>
        public async Task<float> GetAbsoluteThrottlePositionB()
        {
            var text = await this.SendPidCommand(1, 0x47);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetAbsoluteThrottlePositionB()

        /// <summary>
        /// Gets the accelerator pedal position D.
        /// </summary>
        /// <returns>The accelerator pedal position D.</returns>
        public async Task<float> GetAcceleratorPedalPositionD()
        {
            var text = await this.SendPidCommand(1, 0x49);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetAcceleratorPedalPositionD()

        /// <summary>
        /// Gets the accelerator pedal position E.
        /// </summary>
        /// <returns>The accelerator pedal position E.</returns>
        public async Task<float> GetAcceleratorPedalPositionE()
        {
            var text = await this.SendPidCommand(1, 0x4A);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetAcceleratorPedalPositionE()

        /// <summary>
        /// Gets the commanded throttle actuator value.
        /// </summary>
        /// <returns>The commanded throttle actuator value.</returns>
        public async Task<float> GetCommandedThrottleActuator()
        {
            var text = await this.SendPidCommand(1, 0x4C);
            return ((float)ObdSupport.HexToInt(text) * 100) / 255;
        } // GetCommandedThrottleActuator()

        /// <summary>
        /// Gets the engine oil temperature.
        /// </summary>
        /// <returns>The engine oil temperature.</returns>
        public async Task<int> GetEngineOilTemperature()
        {
            var text = await this.SendPidCommand(1, 0x5C);
            return (int)ObdSupport.HexToInt(text) - 40;
        } // GetEngineOilTemperature()

        /// <summary>
        /// Gets the diagnostic trouble codes.
        /// </summary>
        /// <returns>An enumeration of DTCs.</returns>
        public async Task<IEnumerable<ushort>> GetDiagnosticTroubleCodes()
        {
            var list = new List<ushort>();

            var text = await this.SendPidCommand(3, -1);

            var index = 0;
            while (index + 4 <= text.Length)
            {
                var dtcText = text.Substring(index, 4);
                var dtcBcd = (ushort)ObdSupport.HexToInt(dtcText);
                if (dtcBcd > 0)
                {
                    list.Add(dtcBcd);
                } // if
                index += 4;
            } // while

            return list;
        } // GetDiagnosticTroubleCodes()

        /// <summary>
        /// Clears the diagnostic trouble codes.
        /// </summary>
        /// <returns><c>true</c>if the command was successful; 
        /// otherwise <c>false</c></returns>
        public async Task<bool> ClearDiagnosticTroubleCodes()
        {
            var text = await this.SendPidCommand(4, -1);

            return (text == "00");
        } // ClearDiagnosticTroubleCodes()

        /// <summary>
        /// Gets the vehicle identification number.
        /// </summary>
        /// <returns>The vehicle identification number string.</returns>
        public async Task<string> GetVehicleIdentificationNumber()
        {
            var text = await this.SendPidMultiLineCommand(9, 0x02);

            return text;
        } // GetVehicleIdentificationNumber()

        /// <summary>
        /// Gets the calibration id.
        /// </summary>
        /// <returns>The calibration id string.</returns>
        public async Task<string> GetCalibrationId()
        {
            var text = await this.SendPidMultiLineCommand(9, 0x04);

            return text;
        } // GetCalibrationId()

        /// <summary>
        /// Gets the ECU name.
        /// </summary>
        /// <returns>The ECU name string.</returns>
        public async Task<string> GetEcuName()
        {
            var text = await this.SendPidMultiLineCommand(9, 0x0A);

            return text;
        } // GetEcuName()

        /// <summary>
        /// Creates an report of the raw values returned by the OBD device.
        /// </summary>
        /// <param name="output">The output.</param>
        public async void CreateRawReport(Action<string> output)
        {
            output("Cmd='ATI'");
            var text = await this.SendRawCommand("ATI");
            output(text);
            output("----------");

            output("Cmd='ATRV'");
            text = await this.SendRawCommand("ATRV");
            output(text);
            output("----------");

            output("Cmd='ATDP'");
            text = await this.SendRawCommand("ATDP");
            output(text);
            output("----------");

            for (var i = 0x00; i < 0x87 /*87*/; i++)
            {
                await this.PidHelper(1, i, output);
            } // for

            await this.PidHelper(1, 0xA0, output);
            await this.PidHelper(1, 0xC0, output);

            await this.PidHelper(3, -1, output);

            for (var i = 0x0100; i <= 0x0110; i++)
            {
                await this.PidHelper(5, i, output);
            } // for
            for (var i = 0x0200; i <= 0x0210; i++)
            {
                await this.PidHelper(5, i, output);
            } // for

            await this.PidHelper(6, 0, output);
            await this.PidHelper(7, 0, output);
            await this.PidHelper(8, 0, output);

            for (var i = 0x00; i <= 0x0B; i++)
            {
                await this.PidHelper(9, i, output);
            } // for
        } // CreateRawReport()

        /// <summary>
        /// Lists the capabilities.
        /// </summary>
        /// <param name="output">The output.</param>
        public async void ListCapabilities(Action<string> output)
        {
            var supportedPids = await this.GetSupportedPids20();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 1 PIDs (0x01-0x20): {0}", ObdSupport.GetSupportedPidText(supportedPids, 0)));

            supportedPids = await this.GetSupportedPids40();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 1 PIDs (0x21-0x40): {0}", ObdSupport.GetSupportedPidText(supportedPids, 0x20)));

            supportedPids = await this.GetSupportedPids60();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 1 PIDs (0x41-0x60): {0}", ObdSupport.GetSupportedPidText(supportedPids, 0x40)));

            supportedPids = await this.GetSupportedPids80();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 1 PIDs (0x61-0x80): {0}", ObdSupport.GetSupportedPidText(supportedPids, 0x60)));

            supportedPids = await this.GetSupportedPidsA0();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 1 PIDs (0x81-0xA0): {0}", ObdSupport.GetSupportedPidText(supportedPids, 0x80)));

            supportedPids = await this.GetSupportedPidsC0();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 1 PIDs (0xA1-0xC0): {0}", ObdSupport.GetSupportedPidText(supportedPids, 0xA0)));

            supportedPids = await this.GetSupportedPidsE0();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 1 PIDs (0xC1-0xE0): {0}", ObdSupport.GetSupportedPidText(supportedPids, 0xC0)));

            supportedPids = await this.GetSupportedMode5Pids();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 5 PIDs: {0}", ObdSupport.GetSupportedPidText(supportedPids, 0)));

            supportedPids = await this.GetSupportedMode9Pids();
            output(string.Format(CultureInfo.CurrentCulture,
                "Mode 9 PIDs: {0}", ObdSupport.GetSupportedPidText(supportedPids, 0)));
        } // ListCapabilities()

        /// <summary>
        /// Creates the full OBD report and outputs it to debug trace.
        /// </summary>
        /// <param name="output">The output.</param>
        public async void CreateFullReport(Action<string> output)
        {
            output(string.Format(CultureInfo.CurrentCulture,
                "Full OBD Report, {0}", DateTime.Now.ToString("F", CultureInfo.CurrentCulture)));
            output(string.Empty);

            output(string.Format(CultureInfo.CurrentCulture, "OBD Device Name = {0}",
                await this.GetObdDeviceName()));
            output(string.Format(CultureInfo.CurrentCulture, "Battery Voltage = {0}V",
                await this.GetBatteryVoltage()));
            output(string.Format(CultureInfo.CurrentCulture, "Protocol = {0}",
                await this.GetCurrentProtocol()));

            var supportedPids = await this.GetSupportedPids20();
            if (ObdSupport.IsPidSupported(supportedPids, 1, 0))
            {
                var monitorStatus = await this.GetMonitorStatus();
                output(ObdSupport.GetMonitorStatusText(monitorStatus));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 3, 0))
            {
                var fuelSystemStatus = await this.GetFuelSystemStatus();
                switch (fuelSystemStatus)
                {
                    case 1:
                        output("Fuel status: Open loop due to insufficient engine temperature");
                        break;
                    case 2:
                        output("Fuel status: Closed loop, using oxygen sensor feedback to determine fuel mix");
                        break;
                    case 4:
                        output("Fuel status: Open loop due to engine load OR fuel cut due to deceleration");
                        break;
                    case 8:
                        output("Fuel status: Open loop due to system failure");
                        break;
                    case 16:
                        output("Fuel status: Closed loop, using at least one oxygen sensor but there is a fault in the feedback system");
                        break;
                } // switch
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 4, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture, 
                    "Engine load = {0}%",
                    await this.GetCalculatedEngineLoad()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 5, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture, 
                    "Engine coolant temperature = {0}°C",
                    await this.GetEngineCoolantTemperature()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 6, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Short term fuel trim—Bank 1 = {0:N}%",
                    await this.GetShortTermFuelTrim(1)));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 7, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Long term fuel trim—Bank 1 = {0:N}%",
                    await this.GetLongTermFuelTrim(1)));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 8, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Short term fuel trim—Bank 1 = {0:N}%",
                    await this.GetShortTermFuelTrim(2)));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 9, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Long term fuel trim—Bank 1 = {0:N}%",
                    await this.GetLongTermFuelTrim(2)));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x0A, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Fuel pressure = {0} kPa",
                    await this.GetFuelPressure()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x0B, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Intake manifold absolute pressure = {0} kPa",
                    await this.GetIntakeManifoldAbsolutePressure()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x0C, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Engine RPM = {0} rpm",
                    await this.GetEngineRpm()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x0D, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Vehicle speed = {0} km/h",
                    await this.GetVehicleSpeed()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x0E, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Timing advance = {0}° relative to #1 cylinder",
                    await this.GetTimingAdvance()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x0F, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Intake air temperature = {0}°C",
                    await this.GetIntakeAirTemperature()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x10, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "MAF air flow rate = {0:N} grams/sec",
                    await this.GetMafAirFlowRate()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x11, 0))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Throttle position = {0:N}%",
                    await this.GetThrottlePosition()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x13, 0))
            {
                int oxygenSensors = await this.GetOxygenSensorsPresent();
                output(string.Format(CultureInfo.CurrentCulture,
                    "Oxygen sensors present = {0}",
                    ObdSupport.GetOxygenSensorsPresentText((byte)oxygenSensors)));
            } // if

            for (var bank = 1; bank < 3; bank++)
            {
                for (var sensor = 1; sensor < 5; sensor++)
                {
                    var pid = ObdSupport.GetOxygenSensorPid(bank, sensor);
                    if (ObdSupport.IsPidSupported(supportedPids, pid, 0))
                    {
                        output(string.Format(CultureInfo.CurrentCulture,
                            "Oxygen sensor voltage Bank {0}, Sensor {1} = {2}V",
                            bank, sensor, await this.GetOxygenSensorVoltage(bank, sensor)));
                        var fueltrim = await this.GetOxygenSensorFuelTrim(bank, sensor);
                        if (fueltrim > -200)
                        {
                            output(string.Format(CultureInfo.CurrentCulture,
                              "Short term fuel trim Bank {0}, Sensor {1} = {2:N}%",
                              bank, sensor, fueltrim));
                        } // if
                    } // if
                } // for (sensor)
            } // for (bank)

            if (ObdSupport.IsPidSupported(supportedPids, 0x1C, 0))
            {
                var standard = await this.GetCarObdStandard();
                output(string.Format(CultureInfo.CurrentCulture,
                    "OBD standards this vehicle conforms to = {0}",
                    ObdSupport.GetObdStandardText(standard)));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x1F, 0))
            {
                var totalseconds = await this.GetRuntimeSinceEngineStart();
                var hours = totalseconds / 3600;
                var minutes = (totalseconds - (hours * 3600)) / 60;
                var seconds = totalseconds - (hours * 3600) - (minutes * 60);
                output(string.Format(CultureInfo.CurrentCulture,
                    "Run time since engine start = {0:D2}:{1:D2}:{2:D2}",
                    hours, minutes, seconds));
            } // if

            supportedPids = await this.GetSupportedPids40();

            if (ObdSupport.IsPidSupported(supportedPids, 0x21, 0x20))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Distance traveled with malfunction indicator lamp (MIL) on = {0} km",
                    await this.GetDistanceTraveledWithMilOn()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x23, 0x20))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Fuel Rail Pressure = {0:N2} kPa",
                    await this.GetFuelRailPressure()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x2e, 0x20))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Commanded evaporative purge = {0:N2}%",
                    await this.GetCommandedEvaporativePurge()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x2f, 0x20))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Fuel Level Input = {0:N2}%",
                    await this.GetFuelLevelInput()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x30, 0x20))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "# of warm-ups since codes cleared = {0}",
                    await this.GetWarmupsSinceCodesCleared()));
            } // ifv

            if (ObdSupport.IsPidSupported(supportedPids, 0x31, 0x20))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Distance traveled since codes cleared = {0} km",
                    await this.GetDistanceTraveledSinceCodesCleared()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x33, 0x20))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Barometric pressure = {0} kPa",
                    await this.GetBarometricPressure()));
            } // if

            for (var bank = 1; bank < 3; bank++)
            {
                for (var sensor = 1; sensor < 3; sensor++)
                {
                    var pid = ObdSupport.GetCatalystTemperaturePid(bank, sensor);
                    if (ObdSupport.IsPidSupported(supportedPids, pid, 0x20))
                    {
                        output(string.Format(CultureInfo.CurrentCulture,
                            "Catalyst Temperature Bank {0}, Sensor {1} = {2}°C",
                            bank, sensor, await this.GetCatalystTemperature(bank, sensor)));
                    } // if
                } // for (sensor)
            } // for (bank)

            supportedPids = await this.GetSupportedPids60();
            if (ObdSupport.IsPidSupported(supportedPids, 0x41, 0x40))
            {
                var monitorStatus = await this.GetMonitorStatusDriveCycle();
                output(string.Format(CultureInfo.CurrentCulture,
                    "Monitor status this drive cycle = {0:X4}", monitorStatus));
                output(ObdSupport.GetMonitorStatusDriveCycleText(monitorStatus));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x42, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Control module voltage = {0:N2} V",
                    await this.GetControlModuleVoltage()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x43, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Absolute load value = {0:N2}%",
                    await this.GetAbsoluteLoadValue()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x44, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Commanded fuel/air equivalence ratio= {0:N2}",
                    await this.GetCommandedFuelAirRatio()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x45, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Relative throttle position = {0:N2}%",
                    await this.GetRelativeThrottlePosition()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x46, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Ambient air temperature = {0}°C",
                    await this.GetAmbientAirTemperature()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x47, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Absolute throttle position B = {0:N2}%",
                    await this.GetAbsoluteThrottlePositionB()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x49, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Accelerator pedal position D = {0:N2}%",
                    await this.GetAcceleratorPedalPositionD()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x4A, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Accelerator pedal position E = {0:N2}%",
                    await this.GetAcceleratorPedalPositionE()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x4C, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Commanded throttle actuator = {0:N2}%",
                    await this.GetCommandedThrottleActuator()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x5C, 0x40))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Engine oil temperature = {0}°C",
                    await this.GetEngineOilTemperature()));
            } // if

            supportedPids = await this.GetSupportedMode9Pids();
            if (ObdSupport.IsPidSupported(supportedPids, 0x02, 0x00))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Vehicle identification number (VIN) = {0}",
                    await this.GetVehicleIdentificationNumber()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x04, 0x00))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "Calibration ID = {0}",
                    await this.GetCalibrationId()));
            } // if

            if (ObdSupport.IsPidSupported(supportedPids, 0x0A, 0x00))
            {
                output(string.Format(CultureInfo.CurrentCulture,
                    "ECU name = {0}",
                    await this.GetEcuName()));
            } // if
        } // CreateFullReport()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Raises the error encountered event.
        /// </summary>
        /// <param name="error">The error.</param>
        private void RaiseErrorEncountered(string error)
        {
            var copy = this.ErrorEncountered;
            if (copy != null)
            {
                copy(this, error);
            } // if
        } // RaiseErrorEncountered()

        /// <summary>
        /// Helper method for PID queries.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="output">The output.</param>
        /// <returns>The result string.</returns>
        private async Task<string> PidHelper(int mode, int pid, Action<string> output)
        {
            var text = string.Format("Cmd=mode {0}, PID 0x{1:X2}", mode, pid);
            output(text);
            text = await this.SendPidCommand(mode, pid, true);
            output(text);
            output("----------");

            return text;
        } // PidHelper()
        #endregion // PRIVATE METHODS
    } // ObdManagerBase
} // Tethys.OBD
