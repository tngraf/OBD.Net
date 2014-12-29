#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Support for automotive On-board diagnostics (OBD).
//
// ==========================================================================
// <copyright file="MainForm.cs" company="Tethys">
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

namespace Tethys.OBD.ObdSimulator.UI
{
    using System;
    using System.Windows.Forms;

    using Tethys.Logging;
    using Tethys.UiSupport;

    /// <summary>
    /// Implements the main form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Logger for use in this class.
        /// </summary>
        private static ILog log;

        /// <summary>
        /// The simulator.
        /// </summary>
        private ObdSimulatorWin simulator;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            this.ConfigureLogging();
        } // MainForm()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing 
        /// the event data.</param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            this.InitializesComPortList();

            this.EnableControls(false);
            this.txtEngineCoolantTemperature.Text = "80";
            this.txtEngineLoad.Text = "55";
            this.txtEngineRpm.Text = "5800";
            this.txtVehicleSpeed.Text = "130";
        } // MainFormLoad

        /// <summary>
        /// Handles the FormClosing event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance 
        /// containing the event data.</param>
        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.simulator != null)
            {
                try
                {
                    this.simulator.Disconnect();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    // IGNORE
                } // catch
            } // if
        } // MainFormFormClosing()

        /// <summary>
        /// Handles the Click event of the connect button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnConnectClick(object sender, EventArgs e)
        {
            var port = this.comboPort.SelectedItem as string;
            if (string.IsNullOrEmpty(port))
            {
                return;
            } // if

            this.simulator = new ObdSimulatorWin(port);

            try
            {
                this.simulator.Connect();

                log.Info("Connected to port " + port);

                this.EnableControls(true);
                this.UpdateObdValues();
            }
            catch (Exception ex)
            {
                log.Error("Unable to connect: ", ex);
                MessageBox.Show("Unable to connect: " + ex.Message);
            } // catch
        } // BtnConnectClick()

        /// <summary>
        /// Handles the Click event of the Disconnect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnDisconnectClick(object sender, EventArgs e)
        {
            if (this.simulator != null)
            {
                try
                {
                    this.simulator.Disconnect();
                    log.Info("Successfully disconnected.");
                }
                catch (Exception ex)
                {
                    log.Error("Unable to disconnect: ", ex);
                    MessageBox.Show("Unable to disconnect: " + ex.Message);
                } // catch
            } // if

            this.EnableControls(false);
        } // BtnDisconnectClick()

        /// <summary>
        /// Handles the Click event of the <c>btnSetValues</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void BtnSetValuesClick(object sender, EventArgs e)
        {
            this.UpdateObdValues();
        } // BtnSetValuesClick()
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Updates the OBD values.
        /// </summary>
        private void UpdateObdValues()
        {
            try
            {
                var temp = int.Parse(this.txtEngineCoolantTemperature.Text);
                this.simulator.SetEngineCoolantTemperature(temp);

                var load = int.Parse(this.txtEngineLoad.Text);
                this.simulator.SetEngineLoad(load);

                var rpm = int.Parse(this.txtEngineRpm.Text);
                this.simulator.SetEngineRpm(rpm);

                var speed = int.Parse(this.txtVehicleSpeed.Text);
                this.simulator.SetVehicleSpeed(speed);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // IGNORE
            } // catch
        } // UpdateObdValues()

        /// <summary>
        /// Enables the controls.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        private void EnableControls(bool enable)
        {
            this.comboPort.Enabled = !enable;
            this.groupSettings.Enabled = enable;
            this.btnConnect.Enabled = !enable;
            this.btnDisconnect.Enabled = enable;
        } // EnableControls()

        /// <summary>
        /// Initializes the COM port list.
        /// </summary>
        private void InitializesComPortList()
        {
            var portlist = ComPortSupport.GetPortNames(true);

            if (portlist != null)
            {
                // ReSharper disable CoVariantArrayConversion
                this.comboPort.Items.AddRange(portlist);
                // ReSharper restore CoVariantArrayConversion
            } // if

            if (this.comboPort.Items.Count > 0)
            {
                this.comboPort.SelectedIndex = 0;
            } // if
        } // InitializesComPortList()

        /// <summary>
        /// Configures the logging.
        /// </summary>
        private void ConfigureLogging()
        {
            LogManager.Adapter = new LogViewFactoryAdapter(this.rtfLogView);
            log = LogManager.GetLogger(typeof(MainForm));

            this.rtfLogView.AddAtTail = true;
            this.rtfLogView.ShowDebugEvents = true;
        } // ConfigureLogging()
        #endregion // PRIVATE METHODS
    } // MainForm
} // Tethys.OBD.ObdSimulator.UI
