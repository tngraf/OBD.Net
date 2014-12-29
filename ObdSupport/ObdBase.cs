#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdBase.cs" company="Tethys">
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
    using System.Collections.Generic;

    /// <summary>
    /// OBD base support.
    /// </summary>
    public class ObdBase
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// CR/LF required at the end of each ELM command. 
        /// </summary>
        public const string ElmCrLf = "\r\n";

        /// <summary>
        /// Default ELM prompt character ('>').
        /// </summary>
        public const string ElmPrompt = ">";

        /// <summary>
        /// Default timeout in milliseconds (200 milliseconds).
        /// </summary>
        public const int ElmTimeout = 200;

        /// <summary>
        /// ELM "don't know" prompt.
        /// </summary>
        public const string ElmQueryPrompt = "?";

        /// <summary>
        /// ELM "OK" prompt.
        /// </summary>
        public const string ElmOkPrompt = "OK";

        /// <summary>
        /// ELM "NO DATA" prompt (no data for that PID).
        /// </summary>
        public const string ElmNoDataPrompt = "NO DATA";

        /// <summary>
        /// ELM "UNABLE TO CONNECT" prompt (ignition not activated).
        /// </summary>
        public const string ElmUnableToConnect = "UNABLE TO CONNECT";

        /// <summary>
        /// Prefix for AT commands.
        /// </summary>
        public const string ElmAtPrefix = "AT";

        // ----- well known PIDs -----

        /// <summary>
        /// The PID for 'calculated engine load'.
        /// </summary>
        public const int PidCalculatedEngineLoad = 0x04;

        /// <summary>
        /// The PID for 'Engine coolant temperature'.
        /// </summary>
        public const int PidEngineCoolantTemperature = 0x05;

        /// <summary>
        /// The PID for 'Fuel pressure'.
        /// </summary>
        public const int PidFuelPressure = 0x0A;

        /// <summary>
        /// The PID for 'Engine RPM'.
        /// </summary>
        public const int PidEngineRpm = 0x0C;

        /// <summary>
        /// The PID for 'Vehicle speed'.
        /// </summary>
        public const int PidVehicleSpeed = 0x0D;

        /// <summary>
        /// The PID for 'Intake air temperature'.
        /// </summary>
        public const int PidIntakeAirTemperature = 0x0F;

        /// <summary>
        /// The PID for 'Throttle position'.
        /// </summary>
        public const int PidThrottlePosition = 0x11;

        /// <summary>
        /// The PID for 'Runtime'.
        /// </summary>
        public const int PidRuntime = 0x1F;

        /// <summary>
        /// Gets the mode 1 PID descriptions.
        /// </summary>
        public Dictionary<int, string> Mode1PidDescription { get; private set; }

        /// <summary>
        /// Monitor bit D0.
        /// </summary>
        public const int D0 = 0x00000001;

        /// <summary>
        /// Monitor bit D1.
        /// </summary>
        public const int D1 = 0x00000002;

        /// <summary>
        /// Monitor bit D2.
        /// </summary>
        public const int D2 = 0x00000004;

        /// <summary>
        /// Monitor bit D3.
        /// </summary>
        public const int D3 = 0x00000008;

        /// <summary>
        /// Monitor bit D4.
        /// </summary>
        public const int D4 = 0x00000010;

        /// <summary>
        /// Monitor bit D5.
        /// </summary>
        public const int D5 = 0x00000020;

        /// <summary>
        /// Monitor bit D6.
        /// </summary>
        public const int D6 = 0x00000040;

        /// <summary>
        /// Monitor bit D7.
        /// </summary>
        public const int D7 = 0x00000080;

        /// <summary>
        /// Monitor bit C0.
        /// </summary>
        public const int C0 = 0x00000100;

        /// <summary>
        /// Monitor bit C1.
        /// </summary>
        public const int C1 = 0x00000200;

        /// <summary>
        /// Monitor bit C2.
        /// </summary>
        public const int C2 = 0x00000400;

        /// <summary>
        /// Monitor bit C3.
        /// </summary>
        public const int C3 = 0x00000800;

        /// <summary>
        /// Monitor bit C4.
        /// </summary>
        public const int C4 = 0x00001000;

        /// <summary>
        /// Monitor bit C5.
        /// </summary>
        public const int C5 = 0x00002000;

        /// <summary>
        /// Monitor bit C6.
        /// </summary>
        public const int C6 = 0x00004000;

        /// <summary>
        /// Monitor bit C7.
        /// </summary>
        public const int C7 = 0x00008000;

        /// <summary>
        /// Monitor bit B0.
        /// </summary>
        public const int B0 = 0x00010000;

        /// <summary>
        /// Monitor bit B1.
        /// </summary>
        public const int B1 = 0x00020000;

        /// <summary>
        /// Monitor bit B2.
        /// </summary>
        public const int B2 = 0x00040000;

        /// <summary>
        /// Monitor bit B3.
        /// </summary>
        public const int B3 = 0x00080000;

        /// <summary>
        /// Monitor bit B4.
        /// </summary>
        public const int B4 = 0x00100000;

        /// <summary>
        /// Monitor bit B5.
        /// </summary>
        public const int B5 = 0x00200000;

        /// <summary>
        /// Monitor bit B6.
        /// </summary>
        public const int B6 = 0x00400000;

        /// <summary>
        /// Monitor bit B7.
        /// </summary>
        public const int B7 = 0x00800000;

        /// <summary>
        /// Monitor bit A7.
        /// </summary>
        public const uint A7 = 0x80000000;
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ObdBase"/> class.
        /// </summary>
        public ObdBase()
        {
            this.InitMode1PidDescription();
        } // ObdBase()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Initializes the mode 1 PID description dictionary.
        /// </summary>
        private void InitMode1PidDescription()
        {
            this.Mode1PidDescription = new Dictionary<int, string>(50);

            this.Mode1PidDescription.Add(0x00, "PIDs supported [01 - 20]");
            this.Mode1PidDescription.Add(0x01, "Monitor status since DTCs cleared");
            this.Mode1PidDescription.Add(0x02, "Freeze DTC");
            this.Mode1PidDescription.Add(0x03, "Fuel system status");
            this.Mode1PidDescription.Add(0x04, "Calculated engine load value");
            this.Mode1PidDescription.Add(0x05, "Engine coolant temperature");
            this.Mode1PidDescription.Add(0x06, "Short term fuel % trim—Bank 1");
            this.Mode1PidDescription.Add(0x07, "Long term fuel % trim—Bank 1");
            this.Mode1PidDescription.Add(0x08, "Short term fuel % trim—Bank 2");
            this.Mode1PidDescription.Add(0x09, "Long term fuel % trim—Bank 2");
            this.Mode1PidDescription.Add(0x0A, "Fuel pressure");
            this.Mode1PidDescription.Add(0x0B, "Intake manifold absolute pressure");
            this.Mode1PidDescription.Add(0x0C, "Engine RPM");
            this.Mode1PidDescription.Add(0x0D, "Vehicle speed");
            this.Mode1PidDescription.Add(0x0E, "Timing advance");
            this.Mode1PidDescription.Add(0x0F, "Intake air temperature");
            this.Mode1PidDescription.Add(0x10, "MAF air flow rate");
            this.Mode1PidDescription.Add(0x11, "Throttle position");
            this.Mode1PidDescription.Add(0x12, "Commanded secondary air status");
            this.Mode1PidDescription.Add(0x13, "Oxygen sensors present");
            this.Mode1PidDescription.Add(0x14, "Bank 1, Sensor 1: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x15, "Bank 1, Sensor 2: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x16, "Bank 1, Sensor 3: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x17, "Bank 1, Sensor 4: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x18, "Bank 2, Sensor 1: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x19, "Bank 2, Sensor 2: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x1A, "Bank 2, Sensor 3: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x1B, "Bank 2, Sensor 4: Oxygen sensor voltage, Short term fuel trim");
            this.Mode1PidDescription.Add(0x1C, "OBD standards this vehicle conforms to");
            this.Mode1PidDescription.Add(0x1D, "Oxygen sensors present");
            this.Mode1PidDescription.Add(0x1E, "Auxiliary input status");
            this.Mode1PidDescription.Add(0x1F, "Run time since engine start");
            this.Mode1PidDescription.Add(0x20, "PIDs supported [21 - 40]");
            this.Mode1PidDescription.Add(0x21, "Distance traveled with malfunction indicator lamp (MIL) on");
            this.Mode1PidDescription.Add(0x22, "Fuel Rail Pressure (relative to manifold vacuum)");
            this.Mode1PidDescription.Add(0x23, "Fuel Rail Pressure (diesel, or gasoline direct inject)");
            this.Mode1PidDescription.Add(0x24, "O2S1_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x25, "O2S2_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x26, "O2S3_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x27, "O2S4_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x28, "O2S5_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x29, "O2S6_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x2A, "O2S7_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x2B, "O2S8_WR_lambda(1): Equivalence Ratio Voltage");
            this.Mode1PidDescription.Add(0x2C, "Commanded EGR");
            this.Mode1PidDescription.Add(0x2D, "EGR Error");
            this.Mode1PidDescription.Add(0x2E, "Commanded evaporative purge");
            this.Mode1PidDescription.Add(0x2F, "Fuel Level Input");
            this.Mode1PidDescription.Add(0x30, "# of warm-ups since codes cleared ");
            this.Mode1PidDescription.Add(0x31, "Distance traveled since codes cleared");
            this.Mode1PidDescription.Add(0x32, "Evap. System Vapor Pressure");
            this.Mode1PidDescription.Add(0x33, "Barometric pressure");
            this.Mode1PidDescription.Add(0x34, "O2S1_WR_lambda(1): Equivalence Ratio Current");
            this.Mode1PidDescription.Add(0x35, string.Empty);
            this.Mode1PidDescription.Add(0x36, string.Empty);
            this.Mode1PidDescription.Add(0x37, string.Empty);
            this.Mode1PidDescription.Add(0x38, string.Empty);
            this.Mode1PidDescription.Add(0x39, string.Empty);
            this.Mode1PidDescription.Add(0x3A, string.Empty);
            this.Mode1PidDescription.Add(0x3B, string.Empty);
            this.Mode1PidDescription.Add(0x3C, "Catalyst Temperature Bank 1, Sensor 1");
            this.Mode1PidDescription.Add(0x3D, string.Empty);
            this.Mode1PidDescription.Add(0x3E, string.Empty);
            this.Mode1PidDescription.Add(0x3F, string.Empty);
            this.Mode1PidDescription.Add(0x40, "PIDs supported [41 - 60]");
            this.Mode1PidDescription.Add(0x41, "Monitor status this drive cycle");
            this.Mode1PidDescription.Add(0x42, "Control module voltage");
            this.Mode1PidDescription.Add(0x43, "Absolute load value");
            this.Mode1PidDescription.Add(0x44, "Fuel/Air commanded equivalence ratio");
            this.Mode1PidDescription.Add(0x45, "Relative throttle position");
            this.Mode1PidDescription.Add(0x46, "Ambient air temperature");
            this.Mode1PidDescription.Add(0x47, "Absolute throttle position B");
            this.Mode1PidDescription.Add(0x48, "Absolute throttle position C");
            this.Mode1PidDescription.Add(0x49, "Accelerator pedal position D");
            this.Mode1PidDescription.Add(0x4A, "Accelerator pedal position E");
            this.Mode1PidDescription.Add(0x4B, "Accelerator pedal position F");
            this.Mode1PidDescription.Add(0x4C, "Commanded throttle actuator");
            this.Mode1PidDescription.Add(0x4D, "Time run with MIL on");
            this.Mode1PidDescription.Add(0x4E, "Time since trouble codes cleared");
            this.Mode1PidDescription.Add(0x4F, "Maximum value for ...");
        } // InitMode1PidDescription()
        #endregion // PRIVATE METHODS
    } // ObdBase
} // TgSoft.OBD
