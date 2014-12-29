#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A support library for Windows Forms applications.
//
// ==========================================================================
// <copyright file="PortNameComparer.cs" company="Tethys">
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
    using System.Collections;
    using System.Globalization;

    /// <summary>
  /// PortNameComparer implements an IComparer interface
  /// to compare serial port name strings (COM1, COM2, COM10, ...
  /// </summary>
  public class PortNameComparer : IComparer
  {
    #region IComparer Members
    /// <summary>
    /// Compares two serial port name strings and returns a value indicating
    /// whether one is less than, equal to, or greater than the other.
    /// </summary>
    /// <param name="x">The first serial port name to compare.</param>
    /// <param name="y">The second serial port name to compare.</param>
    /// <returns>
    /// Less than zero ==> x is less than y.<br/>
    /// Zero ==> x equals y.<br/>
    /// Greater than zero ==> x is greater than y.
    /// </returns>
    public int Compare(object x, object y)
    {
      int numx = int.Parse(((string)x).Substring(3),
        CultureInfo.InvariantCulture);
      int numy = int.Parse(((string)y).Substring(3),
        CultureInfo.InvariantCulture);

      if (numx < numy)
      {
        // x < y
        return -1;
      } // if

      if (numx > numy)
      {
        return 1;
      } // if

      return 0;
    } // Compare()
    #endregion
  } // PortNameComparer
}

// ==========================
// end of PortNameComparer.cs
// ==========================