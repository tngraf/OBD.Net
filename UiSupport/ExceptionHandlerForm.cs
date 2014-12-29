#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A support library for Windows Forms applications.
//
// ==========================================================================
// <copyright file="ExceptionHandlerForm.cs" company="Tethys">
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
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Displays information about an exception.
    /// </summary>
    public partial class ExceptionHandlerForm : Form
    {
        #region PRIVATE PROPERTIES
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the exception to display.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExceptionHandlerForm"/> class.
        /// </summary>
        public ExceptionHandlerForm()
        {
            this.InitializeComponent();
        } // ExceptionHandlerForm()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// Handles the Load event of the ExceptionHandlerForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        private void ExceptionHandlerForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ApplicationName))
            {
                this.lblException.Text = "Error in application. Please contact your administrator with the following information:";
            }
            else
            {
                this.lblException.Text = string.Format(CultureInfo.CurrentCulture,
                  "Error in application {0}. Please contact your administrator with the following information:",
                  this.ApplicationName);
            } // if

            if (this.Exception != null)
            {
                // ReSharper disable LocalizableElement
                this.lblException.Text += ("\r\n\r\n" + this.Exception.Message);
                // ReSharper restore LocalizableElement
                this.txtDetails.Text = this.GetFormattedExceptionText(this.Exception);
            }
            else
            {
                this.txtDetails.Text = "No more information available.";
            } // if
        } // ExceptionHandlerForm_Load()

        /// <summary>
        /// Handles the Activated event of the ExceptionHandlerForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing 
        /// the event data.</param>
        private void ExceptionHandlerForm_Activated(object sender, EventArgs e)
        {
            this.btnCancel.Focus();
        } // ExceptionHandlerForm_Activated()

        /// <summary>
        /// Handles the Click event of the save button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            this.SaveToTextFile();
        } // BtnSaveClick()
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Gets the formatted exception text.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>The formatted text.</returns>
        public string GetFormattedExceptionText(Exception ex)
        {
            if (ex != null)
            {
                return string.Format(CultureInfo.CurrentCulture,
                  "{0}: {1}\r\n\r\nStack Trace:\r\n{2}",
                  this.Exception.GetType(), this.Exception.Message,
                  this.Exception.StackTrace);
            } // if
            return "No more information available.";
        } // GetFormattedExceptionText()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Saves the contents of the event log to a text file.
        /// </summary>
        private void SaveToTextFile()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.InitialDirectory = ".";
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            DialogResult result = saveFileDialog.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                // aborted by user
                return;
            } // if

            this.WriteTextFile(saveFileDialog.FileName);
        } // SaveToTextFile()

        /// <summary>
        /// Write the log window contents to a plain text file.
        /// </summary>
        /// <param name="fileName">Name of the file to write to.</param>
        private void WriteTextFile(string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                sw.Write(string.Format(CultureInfo.CurrentCulture,
                  "{0}, {1}", DateTime.Now.ToLongDateString(),
                  DateTime.Now.ToLongTimeString()));
                sw.WriteLine(string.Empty);
                sw.WriteLine("ProductName: " + Application.ProductName);
                sw.WriteLine("ProductVersion: " + Application.ProductVersion);
                sw.WriteLine("StartupPath: " + Application.StartupPath);
                sw.WriteLine("CurrentCulture: " + Application.CurrentCulture);
                sw.WriteLine("CurrentInputLanguage: "
                  + Application.CurrentInputLanguage.Culture);
                sw.WriteLine("UserAppDataPath: " + Application.UserAppDataPath);
                sw.WriteLine(string.Empty);
                sw.WriteLine(this.GetFormattedExceptionText(this.Exception));
                sw.Flush();
            } // using
        } // WriteTextFile()
        #endregion // PRIVATE METHODS
    } // ExceptionHandlerForm
}
