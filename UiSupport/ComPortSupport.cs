#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A support library for Windows Forms applications.
//
// ==========================================================================
// <copyright file="ComPortSupport.cs" company="Tethys">
// Copyright  1998 - 2014 by T. Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Microsoft .Net Framework 4.5. 
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.UiSupport
{
    using System;
    using System.Collections;
    using System.IO.Ports;

    /// <summary>
    /// Support methods for serial ports.
    /// </summary>
    public class ComPortSupport
    {
        /// <summary>
        /// Gets an array or serial port names for the current computer.
        /// </summary>
        /// <param name="onlyComPorts">Filters the available port so that
        /// only COM ports are returned.</param>
        /// <returns>An array or serial port names or null.</returns>
        public static string[] GetPortNames(bool onlyComPorts)
        {
            var portlist = SerialPort.GetPortNames();
            var plist = new ArrayList(portlist);
            if (plist.Count < 1)
            {
                return null;
            } // if

            // take care of non COM-ports (ie. AVMxxx)
            if (onlyComPorts)
            {
                int i = 0;
                do
                {
                    var str = (string)plist[i];
                    if (!str.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
                    {
                        plist.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    } // if
                }
                while (i < plist.Count);
            } // if

            if (plist.Count > 0)
            {
                plist.Sort(new PortNameComparer());
                return (string[])plist.ToArray(typeof(string));
            } // if

            return null;
        } // GetPortNames()
    }
}
