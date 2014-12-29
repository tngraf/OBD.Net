#region Header
// --------------------------------------------------------------------------
// OBD Analyzer
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="Program.cs" company="Tethys">
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

namespace Tethys.OBD.ObdAnalyzer.Net
{
    using System;
    using System.Windows.Forms;

    using Tethys.OBD.ObdAnalyzer.Net.UI;
    using Tethys.UiSupport;

    /// <summary>
    /// Startup class of the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var eh = new CustomExceptionHandler();
            Application.ThreadException += eh.OnThreadException;

            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException
              += OnCurrentDomainUnhandledException;

            Application.Run(new MainForm());
        } // Main()

        /// <summary>
        /// Handle the UI exceptions by showing a dialog box, and asking the user whether
        /// or not they wish to abort execution.
        /// NOTE: This exception cannot be kept from terminating the application - it can only
        /// log the event, and inform the user about it.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance
        /// containing the event data.</param>
        private static void OnCurrentDomainUnhandledException(object sender,
          UnhandledExceptionEventArgs e)
        {
            var result = CustomExceptionHandler.ShowThreadExceptionDialog(
              (Exception)e.ExceptionObject);
            if (result == DialogResult.Abort)
            {
                Application.Exit();
            } // if
        } // OnCurrentDomainUnhandledException()
    } // Program
}
