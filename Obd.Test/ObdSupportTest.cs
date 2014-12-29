#region Header
// --------------------------------------------------------------------------
// OBD Unit Tests
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="ObdSupportTest.cs" company="Tethys">
// Copyright  2014 by Thomas Graf
//            All rights reserved.
// </copyright>
// 
// Version .. 00.01.00.00 of 14Nov23
// System ... Portable Library
// Tools .... Microsoft Visual Studio 2013
//
// Change Report
// 14Nov22 0.01.00.00 tg: initial version.
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
    public class ObdSupportTest
    {
        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestByteToHex()
        {
            var actual = ObdSupport.ByteToHex(0);
            Assert.AreEqual("00", actual);

            actual = ObdSupport.ByteToHex(0x55);
            Assert.AreEqual("55", actual);

            actual = ObdSupport.ByteToHex(255);
            Assert.AreEqual("FF", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestInt16ToHex()
        {
            var actual = ObdSupport.Int16ToHex(0, false);
            Assert.AreEqual("0000", actual);

            actual = ObdSupport.Int16ToHex(0, true);
            Assert.AreEqual("00 00", actual);

            actual = ObdSupport.Int16ToHex(0x1234, false);
            Assert.AreEqual("1234", actual);

            actual = ObdSupport.Int16ToHex(0x1234, true);
            Assert.AreEqual("12 34", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestInt32ToHex()
        {
            var actual = ObdSupport.Int32ToHex(0, false);
            Assert.AreEqual("00000000", actual);

            actual = ObdSupport.Int32ToHex(0, true);
            Assert.AreEqual("00 00 00 00", actual);

            actual = ObdSupport.Int32ToHex(0x76543210, false);
            Assert.AreEqual("76543210", actual);

            actual = ObdSupport.Int32ToHex(0x76543210, true);
            Assert.AreEqual("76 54 32 10", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void HexToInt()
        {
            var actual = ObdSupport.HexToInt("0");
            Assert.AreEqual(0, (int)actual);

            actual = ObdSupport.HexToInt("0000");
            Assert.AreEqual(0, (int)actual);

            actual = ObdSupport.HexToInt("12");
            Assert.AreEqual((uint)18, actual);

            actual = ObdSupport.HexToInt("FFFF");
            Assert.AreEqual((uint)65535, actual);

            actual = ObdSupport.HexToInt("FF FF FF");
            Assert.AreEqual((uint)0xffffff, actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void HexToIntInvalid()
        {
            // NO exception
            var actual = ObdSupport.HexToInt("FFyF");
            Assert.AreEqual(0, (int)actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetEngineLoad()
        {
            var actual = ObdSupport.GetCalculatedEngineLoad("00");
            Assert.AreEqual(0, actual);

            actual = ObdSupport.GetCalculatedEngineLoad("80");
            Assert.AreEqual(50, actual);

            actual = ObdSupport.GetCalculatedEngineLoad("FF");
            Assert.AreEqual(100, actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetOxygenSensorPid()
        {
            var actual = ObdSupport.GetOxygenSensorPid(1, 1);
            Assert.AreEqual(0x14, actual);

            actual = ObdSupport.GetOxygenSensorPid(1, 2);
            Assert.AreEqual(0x15, actual);

            actual = ObdSupport.GetOxygenSensorPid(1, 4);
            Assert.AreEqual(0x17, actual);

            actual = ObdSupport.GetOxygenSensorPid(2, 1);
            Assert.AreEqual(0x18, actual);

            actual = ObdSupport.GetOxygenSensorPid(2, 4);
            Assert.AreEqual(0x1B, actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetOxygenSensorsPresentSensorText()
        {
            var actual = ObdSupport.GetOxygenSensorsPresentSensorText(0x00);
            Assert.AreEqual(string.Empty, actual);

            actual = ObdSupport.GetOxygenSensorsPresentSensorText(0x01);
            Assert.AreEqual("1", actual);

            actual = ObdSupport.GetOxygenSensorsPresentSensorText(0x03);
            Assert.AreEqual("1, 2", actual);

            actual = ObdSupport.GetOxygenSensorsPresentSensorText(0x0F);
            Assert.AreEqual("1, 2, 3, 4", actual);

            actual = ObdSupport.GetOxygenSensorsPresentSensorText(0x09);
            Assert.AreEqual("1, 4", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetOxygenSensorsPresentText()
        {
            var actual = ObdSupport.GetOxygenSensorsPresentText(0x00);
            Assert.AreEqual(string.Empty, actual);

            actual = ObdSupport.GetOxygenSensorsPresentText(0x08);
            Assert.AreEqual("Bank 1, sensors 4", actual);

            actual = ObdSupport.GetOxygenSensorsPresentText(0x09);
            Assert.AreEqual("Bank 1, sensors 1, 4", actual);

            actual = ObdSupport.GetOxygenSensorsPresentText(0x80);
            Assert.AreEqual("Bank 2, sensors 4", actual);

            actual = ObdSupport.GetOxygenSensorsPresentText(0xFE);
            Assert.AreEqual("Bank 1, sensors 2, 3, 4, Bank 2, sensors 1, 2, 3, 4", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetDtcText()
        {
            var actual = ObdSupport.GetDtcText(0x0133);
            Assert.AreEqual("P0133", actual);

            actual = ObdSupport.GetDtcText(0x3133);
            Assert.AreEqual("P3133", actual);

            actual = ObdSupport.GetDtcText(0x4133);
            Assert.AreEqual("C0133", actual);

            actual = ObdSupport.GetDtcText(0x8133);
            Assert.AreEqual("B0133", actual);

            actual = ObdSupport.GetDtcText(0xC133);
            Assert.AreEqual("U0133", actual);

            actual = ObdSupport.GetDtcText(0xF369);
            Assert.AreEqual("U3369", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestIsPidSupported()
        {
            const uint SupportedPids = 0xBE1FA813;

            var actual = ObdSupport.IsPidSupported(SupportedPids, 1, 0);
            Assert.IsTrue(actual);

            actual = ObdSupport.IsPidSupported(SupportedPids, 2, 0);
            Assert.IsFalse(actual);

            actual = ObdSupport.IsPidSupported(SupportedPids, 8, 0);
            Assert.IsFalse(actual);

            actual = ObdSupport.IsPidSupported(SupportedPids, 0x1E, 0);
            Assert.IsFalse(actual);

            actual = ObdSupport.IsPidSupported(SupportedPids, 0x1F, 0);
            Assert.IsTrue(actual);

            actual = ObdSupport.IsPidSupported(SupportedPids, 0x20, 0);
            Assert.IsTrue(actual);
        } // TestIsPidSupported()

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetSupportedPidText()
        {
            var actual = ObdSupport.GetSupportedPidText(0x0, 0);
            Assert.AreEqual("None", actual);

            actual = ObdSupport.GetSupportedPidText(0x1, 0);
            Assert.AreEqual("0x20", actual);

            actual = ObdSupport.GetSupportedPidText(0x80000000, 0);
            Assert.AreEqual("0x01", actual);

            actual = ObdSupport.GetSupportedPidText(0xC0000001, 0);
            Assert.AreEqual("0x01, 0x02, 0x20", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetChar()
        {
            var actual = ObdSupport.GetChar("00");
            Assert.AreEqual('\0', actual);

            actual = ObdSupport.GetChar(":1");
            Assert.AreEqual((char)0xff, actual);

            actual = ObdSupport.GetChar("30");
            Assert.AreEqual('0', actual);

            actual = ObdSupport.GetChar("41");
            Assert.AreEqual('A', actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetMultilineCanString()
        {
            const string DataVin = "0140:4902013144341:475030305235352:42313233343536";
            var actual = ObdSupport.GetMultilineCanString(DataVin, "4902", 17);
            Assert.AreEqual("1D4GP00R55B123456", actual);

            const string DataEcu = "0170:490A0145434D1:002D456E67696E2:65436F6E74726F3:6C000000000000";
            actual = ObdSupport.GetMultilineCanString(DataEcu, "490A", 24);
            Assert.AreEqual("ECM-EngineControl", actual);
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        [TestMethod]
        public void TestGetLastAnswer()
        {
            const string Test1 = "010C\r\n410C0fA0\r\n>";
            var actual = ObdSupport.GetLastAnswer(Test1);
            Assert.AreEqual(Test1, actual);

            const string Test2 = "010C\r\n410C0fA0\r\n>010D\r\n410D0f\r\n>";
            actual = ObdSupport.GetLastAnswer(Test2);
            Assert.AreEqual("010D\r\n410D0f\r\n>", actual);

            actual = ObdSupport.GetLastAnswer(string.Empty);
            Assert.AreEqual(string.Empty, actual);

            actual = ObdSupport.GetLastAnswer(ObdBase.ElmPrompt);
            Assert.AreEqual(ObdBase.ElmPrompt, actual);

            actual = ObdSupport.GetLastAnswer("1>22>333>4444>");
            Assert.AreEqual("4444>", actual);
        } 
    } // ObdSupportTest
} // Tethys.OBD.Test
