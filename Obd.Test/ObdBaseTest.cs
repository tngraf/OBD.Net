#region Header
// --------------------------------------------------------------------------
// OBD Unit Tests
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdBaseTest.cs" company="Tethys">
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

namespace Tethys.OBD.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Tethys.OBD;

    /// <summary>
    /// Unit test class.
    /// </summary>
    [TestClass]
    public class ObdBaseTest
    {
        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var target = new ObdBase();

            Assert.IsNotNull(target.Mode1PidDescription);

            var pid = 0x00;
            Assert.IsTrue(target.Mode1PidDescription.ContainsKey(pid));
            var actual = target.Mode1PidDescription[pid];
            Assert.IsFalse(string.IsNullOrEmpty(actual));

            pid = 0x01;
            Assert.IsTrue(target.Mode1PidDescription.ContainsKey(pid));
            actual = target.Mode1PidDescription[pid];
            Assert.IsFalse(string.IsNullOrEmpty(actual));

            pid = 0x4f;
            Assert.IsTrue(target.Mode1PidDescription.ContainsKey(pid));
            actual = target.Mode1PidDescription[pid];
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        } // TestConstructor()
    } // ObdBaseTest
} // Tethys.OBD.Test
