#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A support library for Windows Forms applications.
//
// ==========================================================================
// <copyright file="CustomExceptionHandler.cs" company="Tethys">
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

#region Additional Information
/* ===========================================================================

 * This file contains the implementation of the class CustomExceptionHandler
which supplies a new general exception handler for .Net applications. If this
exception handler is installed in an application by adding the following lines
of code to Main():

CustomExceptionHandler eh = new CustomExceptionHandler();
Application.ThreadException +=
 new ThreadExceptionEventHandler(eh.OnThreadException);

you will get your own final exception handler instead of a crashed
application.

============================================================================*/
#endregion

namespace Tethys.UiSupport
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Windows.Forms;

    using Tethys.Logging;

    /// <summary>
    /// The Error Handler class
    /// We need a class because event handling methods can't be static.
    /// </summary>
    public class CustomExceptionHandler
    {
        /// <summary>
        /// Logger for use in this class.
        /// </summary>
        private static ILog log;

        /// <summary>
        /// Handle the exception event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="ThreadExceptionEventArgs"/>
        /// instance containing the event data.</param>
        [SuppressMessage("Microsoft.Design",
          "CA1031:DoNotCatchGeneralExceptionTypes",
          Justification = "Ok for top UI level.")]
        public void OnThreadException(object sender,
          ThreadExceptionEventArgs eventArgs)
        {
            log = LogManager.GetLogger(typeof(CustomExceptionHandler));
            log.Error(eventArgs.Exception);

            var result = DialogResult.Cancel;
            try
            {
                result = ShowThreadExceptionDialog(eventArgs.Exception);
            }
            catch
            {
                try
                {
                    MessageBox.Show("Severe Error", "Severe Error",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            } // catch

            if (result == DialogResult.Abort)
            {
                Application.Exit();
            } // if
        } // OnThreadException()

        /// <summary>
        /// Display a dialog indicating the exception to the user.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>A dialog result.</returns>
        public static DialogResult ShowThreadExceptionDialog(Exception e)
        {
            log = LogManager.GetLogger(typeof(CustomExceptionHandler));
            log.Error(e);

            var form = new ExceptionHandlerForm();
            form.ApplicationName = Application.ProductName;
            form.Exception = e;
            return form.ShowDialog();
        } // ShowThreadExceptionDialog()
    } // CustomExceptionHandler
}

// ================================
// End of CustomExceptionHandler.cs
// ================================
