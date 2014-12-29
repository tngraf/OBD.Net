#region Header
// --------------------------------------------------------------------------
// OBD Analyzer
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
// System ... Microsoft .Net Framework 4.5. 
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

#define SERIAL_PORT_MOCK

namespace Tethys.OBD.ObdAnalyzer.Net.UI
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.IO.Ports;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using Tethys.Logging;
    using Tethys.OBD.ObdAnalyzer.Net.Core;
    using Tethys.OBD.ObdAnalyzer.Net.Properties;
    using Tethys.Reflection;
    using Tethys.UiSupport;

    /// <summary>
    /// Implements the main form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The refresh time in milliseconds (1000).
        /// </summary>
        private const int RefreshTime = 200;

        /// <summary>
        /// The simulator port name.
        /// </summary>
        private const string SimulatorPort = "Simulator";

        /// <summary>
        /// Logger for use in this class.
        /// </summary>
        private static ILog log;

        /// <summary>
        /// OBD core functionality.
        /// </summary>
        private ObdManager obdManager;

        /// <summary>
        /// Low level communication for device data upload.
        /// </summary>
        private ILowLevelCom lowcomConfig;

        /// <summary>
        /// The refresh timer.
        /// </summary>
        private Timer refreshTimer;

        /// <summary>
        /// The result builder.
        /// </summary>
        private StringBuilder resultBuilder;

        /// <summary>
        /// The stream writer.
        /// </summary>
        private StreamWriter streamwriter;
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

            this.InitUI();

            this.ConfigureLogging();

            log.Info(Application.ProductName + ", Version " + Application.ProductVersion);
            var assembly = this.GetType().Assembly;
            var version = assembly.GetName().Version;
            log.Debug("Internal Version: " + VersionInfo.GetVersion(assembly,
              version, CultureInfo.CurrentUICulture));
#if DEBUG
            log.Debug("Debug version build");
#endif
            log.Debug("Startup Path: " + Application.StartupPath);
            log.Debug("Executable Path: " + Application.ExecutablePath);
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
            log.Debug("Application starting up...");

            this.InitializesComPortList();

            this.UpdateUI();

            // hide debug trace
            this.splitContainer.Panel2Collapsed = true;

#if DEBUG
            this.MenuToolsDebugTraceClick(this, EventArgs.Empty);
#endif
        } // MainFormLoad

        /// <summary>
        /// Handles the FormClosing event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance 
        /// containing the event data.</param>
        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            log.Info("Application closing down...");
#if false
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
#endif
            e.Cancel = false;
        } // MainFormFormClosing()

        /// <summary>
        /// This function is called when any toolbar button has been clicked.
        /// We are using a trick to perform the action that is associated with the
        /// button: during initialization we assign the Tag property of the button
        /// the menu item that does the real work. In the Click-handler we only
        /// access the Tag and force a simulated click on this menu item.
        /// </summary>
        /// <param name="sender">Originator of this event.</param>
        /// <param name="e">The EventArgs that contains the event data.</param>
        private void ToolStripItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem is ToolStripSeparator)
            {
                return;
            } // if
            var tsb = (ToolStripButton)e.ClickedItem;
            var mi = (ToolStripMenuItem)tsb.Tag;

            if (mi != null)
            {
                mi.PerformClick();
            } // if
        } // ToolStripItemClicked()

        //// ---------------------------------------------------------------------

        #region MENU HANDLING
        #region FILE MENU
        /// <summary>
        /// Handles a click on the start menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MenuFileStartClick(object sender, EventArgs e)
        {
            var port = this.toolComboPorts.SelectedItem as string;
            if (string.IsNullOrEmpty(port))
            {
                return;
            } // if

            this.OpenConnection(port);
            this.UpdateUI();

            this.refreshTimer = new Timer();
            this.refreshTimer.Interval = RefreshTime;
            this.refreshTimer.Tick += this.RefreshTimerOnTick;
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Start();
        } // MenuFileStartClick()

        /// <summary>
        /// Handles the Click event of the Disconnect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MenuFileStopClick(object sender, EventArgs e)
        {
            this.CloseConnection();
            this.UpdateUI();
            this.refreshTimer.Stop();
            this.refreshTimer.Tick -= this.RefreshTimerOnTick;
            this.CloseStatisticsFile();
            this.btnSaveStatistics.Text = "Save Statistics to File";
        } // MenuFileStopClick()

        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the
        /// event data.</param>
        private void MenuFileExitClick(object sender, EventArgs e)
        {
            // Application.Exit();
            this.Close();
        } // MenuFileExitClick()
        #endregion // FILE MENU

        #region TOOLS MENU
        /// <summary>
        /// Shows of hides the debug trace window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        private void MenuToolsDebugTraceClick(object sender, EventArgs e)
        {
            this.menuToolsDebugTrace.Checked = !this.menuToolsDebugTrace.Checked;
            this.splitContainer.Panel2Collapsed = !this.menuToolsDebugTrace.Checked;
        } // MenuToolsDebugTraceClick()
        #endregion //TOOLS MENU

        #region HELP MENU
        /// <summary>
        /// Displays the about box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        private void MenuHelpAboutClick(object sender, EventArgs e)
        {
            var sb = new StringBuilder(255);
            sb.Append("Version");
            sb.Append(" ");
            var assembly = this.GetType().Assembly;
            var version = assembly.GetName().Version;
            sb.Append(VersionInfo.GetVersion(assembly, version,
              CultureInfo.CurrentUICulture));

            var copyright =
              (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly,
              typeof(AssemblyCopyrightAttribute));
            Debug.Assert(copyright != null, "copyright must not be null!");

            var form = new AboutBox();
            form.Title = Application.ProductName;
            form.Copyright = copyright.Copyright;
            form.Version = sb.ToString();
            form.Image = this.Icon.ToBitmap();
            form.Description = "A simple UI to show OBD values.";

            form.ShowDialog(this);
        } // MenuHelpAboutClick()
        #endregion // MENU HELP
        #endregion // MENU HANDLING

        //// ---------------------------------------------------------------------

        /// <summary>
        /// Handles the Click event of the <c>btnGetBatteryVoltage</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing 
        /// the event data.</param>
        private async void BtnGetBatteryVoltageClick(object sender, EventArgs e)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                var val = await this.obdManager.GetBatteryVoltage();
                this.txtBatteryVoltage.Text = string.Format(CultureInfo.CurrentCulture, "{0}V", val);
            }
            catch (Exception ex)
            {
                log.Error("Error refreshing batter voltage", ex);
            } // catch
        } // BtnGetBatteryVoltageClick()

        /// <summary>
        /// Handles the Click event of the <c>btnSendRawCommand</c> control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing 
        /// the event data.</param>
        private async void BtnSendRawCommandClick(object sender, EventArgs e)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                this.txtReply.Text = await this.obdManager.SendRawCommand(this.txtRawCommand.Text);
            }
            catch (Exception ex)
            {
                log.Error("Error sending raw command", ex);
            } // catch
        } // BtnSendRawCommandClick()

        /// <summary>
        /// Handles the Click event of the <c>btnSendPidCommand</c> control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing 
        /// the event data.</param>
        private async void BtnSendPidCommandClick(object sender, EventArgs e)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                var mode = int.Parse(this.txtMode.Text);
                var pid = int.Parse(this.txtPid.Text);
                this.txtReply.Text = await this.obdManager.SendRawPidCommand(mode, pid);
            }
            catch (Exception ex)
            {
                log.Error("Error seding PID command", ex);
            } // catch
        } // BtnSendPidCommandClick()

        /// <summary>
        /// Handles the Click event of the <c>btnCreateFullReport</c> control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing 
        /// the event data.</param>
        private void BtnCreateFullReportClick(object sender, EventArgs e)
        {
            try
            {
                this.resultBuilder = new StringBuilder(1000);
                this.obdManager.CreateFullReport(this.OutputResultBuilder);

                var resultForm = new ResultForm(this.resultBuilder.ToString());
                resultForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.Error("Error creating full report", ex);
            } // catch
        } // BtnCreateFullReportClick()

        /// <summary>
        /// Handles the Click event of the <c>btnRefreshSensorData</c> control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing 
        /// the event data.</param>
        private void BtnRefreshSensorDataClick(object sender, EventArgs e)
        {
            this.RefreshSensorData();
        } // BtnRefreshSensorDataClick()

        /// <summary>
        /// Handles the Check Changed event of the <c>checkAutoRefreshSensorData</c> control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing 
        /// the event data.</param>
        private void CheckAutoRefreshSensorDataCheckedChanged(object sender, EventArgs e)
        {
            this.btnRefreshSensorData.Enabled = !this.checkAutoRefreshSensorData.Checked;
        } // CheckAutoRefreshSensorDataCheckedChanged()

        /// <summary>
        /// Handles the Click event of the <c>btnCreateRawReport</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void BtnCreateRawReportClick(object sender, EventArgs e)
        {
            this.CreateRawReport();
        } // BtnCreateRawReportClick()

        /// <summary>
        /// Handles the Click event of the <c>btnReadDtc</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private async void BtnReadDtcClick(object sender, EventArgs e)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                var monitorStatus = await this.obdManager.GetMonitorStatus();
                var milOn = ((monitorStatus & ObdBase.B7) > 0);
                this.picMil.Image = milOn ? Resources.Check_Engine_70 : Resources.Check_Engine_gray_70;
                this.lblMil.Text = string.Format("MIL (malfunction indicator lamp) is {0}", milOn ? "ON" : "OFF");
                var numDtcs = (monitorStatus & 0x7F000000) >> 24;
                if (numDtcs == 0)
                {
                    lblNumDtc.Text = "No DTCs";
                    return;
                } // if
                lblNumDtc.Text = string.Format("#DTC = {0}", numDtcs);

                var dtcList = await this.obdManager.GetDiagnosticTroubleCodes();
                this.listViewDtc.Items.Clear();
                foreach (var dtc in dtcList)
                {
                    var dtcText = ObdSupport.GetDtcText(dtc);
                    var dtcDescription = DtcDatabase.GetDtcDescription(dtcText);
                    var item = new ListViewItem(dtcText);
                    item.SubItems.Add(dtcDescription);
                    this.listViewDtc.Items.Add(item);
                } // foreach
            }
            catch (Exception ex)
            {
                log.Error("Error reading DTC data", ex);
            } // catch
        } // BtnReadDtcClick()

        /// <summary>
        /// Handles the Click event of the <c>btnClearDtcCodes</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private async void BtnClearDtcCodesClick(object sender, EventArgs e)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            if (MessageBox.Show(this,
                "Do you really want to clear all pending DTCs and the MIL flag?",
                this.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await this.obdManager.ClearDiagnosticTroubleCodes();
                }
                catch (Exception ex)
                {
                    log.Error("Error clearing DTC codes", ex);
                } // catch
            } // if
        } // BtnClearDtcCodesClick()

        /// <summary>
        /// Handles the Click event of the <c>btnListCapabilities</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void BtnListCapabilitiesClick(object sender, EventArgs e)
        {
            this.ListCapabilities();
        } // BtnListCapabilitiesClick()

        /// <summary>
        /// Handles the Click event of the <c>btnSaveStatistics</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void BtnSaveStatisticsClick(object sender, EventArgs e)
        {
            if (this.streamwriter == null)
            {
                this.CreateStatisticsFile();
                this.btnSaveStatistics.Text = "Close Statistics File";
            }
            else
            {
                this.CloseStatisticsFile();
                this.btnSaveStatistics.Text = "Save Statistics to File";
            } // if
        } // BtnSaveStatisticsClick()
        #endregion

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Creates the statistics file.
        /// </summary>
        private void CreateStatisticsFile()
        {
            var filedlg = new SaveFileDialog();
            filedlg.Filter
                = @"CSV Files (*.csv) |*.csv|Text Files (*.txt) |*.txt| All Files (*.*) |*.*||";
            filedlg.FilterIndex = 1;

            if (filedlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            } // if

            try
            {
                this.streamwriter = new StreamWriter(
                    File.OpenWrite(filedlg.FileName));
            }
            catch (Exception ex)
            {
                log.Error("Error opening file", ex);
                this.streamwriter = null;
            } // catch

            this.WriteToStatisticsFile(this.GetStatisticsHeader());
        } // CreateStatisticsFile()

        /// <summary>
        /// Closes the statistics file.
        /// </summary>
        private void CloseStatisticsFile()
        {
            if (this.streamwriter != null)
            {
                this.streamwriter.Flush();
                this.streamwriter.Close();
                this.streamwriter = null;
            } // if
        } // CloseStatisticsFile()

        /// <summary>
        /// Writes the specified text to the statistics file.
        /// </summary>
        /// <param name="text">The text.</param>
        private void WriteToStatisticsFile(string text)
        {
            if (this.streamwriter != null)
            {
                this.streamwriter.Write(text);
                this.streamwriter.Write("\r\n");
                this.streamwriter.Flush();
            } // if
        } // WriteToStatisticsFile()

        /// <summary>
        /// Gets the statistics header.
        /// </summary>
        /// <returns>A CSV header string</returns>
        private string GetStatisticsHeader()
        {
            var sb = new StringBuilder(200);
            sb.Append("Time;");
            for (var i = 0; i < this.listViewSensor.Items.Count; i++)
            {
                var item = this.listViewSensor.Items[i];
                if (item.Checked)
                {
                    sb.Append(item.Text);
                    sb.Append(";");
                } // if
            } // for

            return sb.ToString();
        } // GetStatisticsHeader()

        /// <summary>
        /// Refreshes the sensor data.
        /// </summary>
        private async void RefreshSensorData()
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            // Torque seems to write only 
            // - Speed, Coolant, Throttle, RPM
            // => proposed: real time stamp, engine load
            // NOT: Battery Voltage, Throttle position, Fuel Level Input, Barometric pressure
            // Absolute load value, Relative throttle position, Absolute throttle position B,
            // Commanded throttle actuator
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                var sb = new StringBuilder(200);
                sb.AppendFormat("{0};", DateTime.Now.ToString("u"));

                var supportedPids = await this.obdManager.GetSupportedPids20();
                if (this.listViewSensor.Items.Count == 0)
                {
                    this.UpdateSensorList(supportedPids);
                } // if

                var itemCount = 0;
                if ((ObdSupport.IsPidSupported(supportedPids, 4, 0)) && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetCalculatedEngineLoad();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture, "{0}%", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 5, 0)) && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetEngineCoolantTemperature();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0}°C", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if (this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetBatteryVoltage();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0}V", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x0C, 0)) && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetEngineRpm();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0} rpm", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x0d, 0)) && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetVehicleSpeed();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0} km/h", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x0f, 0)) && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetIntakeAirTemperature();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0}°C", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x11, 0)) && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetThrottlePosition();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0:N}%", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x1f, 0)) && this.listViewSensor.Items[itemCount].Checked)
                {
                    var totalseconds = await this.obdManager.GetRuntimeSinceEngineStart();
                    var hours = totalseconds / 3600;
                    var minutes = (totalseconds - (hours * 3600)) / 60;
                    var seconds = totalseconds - (hours * 3600) - (minutes * 60);

                    var time = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

                    this.listViewSensor.Items[itemCount].SubItems[1].Text = time;
                    sb.AppendFormat("{0};", time);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x2f, 0x20))
                    && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetFuelLevelInput();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0:N2}%", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x33, 0x20))
                    && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetBarometricPressure();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0} kPa", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x43, 0x40))
                    && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetAbsoluteLoadValue();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0:N}%", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x45, 0x40))
                    && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetRelativeThrottlePosition();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0:N}%", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x46, 0x40))
                    && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetAmbientAirTemperature();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0}°C", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x47, 0x40))
                    && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetAbsoluteThrottlePositionB();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0:N}%", val);
                    sb.AppendFormat("{0};", val);
                } // if

                itemCount++;
                if ((ObdSupport.IsPidSupported(supportedPids, 0x4C, 0x40))
                    && this.listViewSensor.Items[itemCount].Checked)
                {
                    var val = await this.obdManager.GetCommandedThrottleActuator();
                    this.listViewSensor.Items[itemCount].SubItems[1].Text = string.Format(
                        CultureInfo.CurrentCulture,
                        "{0:N}%", val);
                    sb.AppendFormat("{0};", val);
                } // if

                this.lblLastUpdate.Text = DateTime.Now.ToLongTimeString();

                if (this.streamwriter != null)
                {
                    this.WriteToStatisticsFile(sb.ToString());
                } // if
            }
            catch (Exception ex)
            {
                log.Error("Error refreshing sensor data", ex);
            } // catch

            watch.Stop();

            // cycle time = approx 100ms with simulator
            ////log.InfoFormat("Cycle time = {0} ms", watch.ElapsedMilliseconds);
        } // RefreshSensorData()

        /// <summary>
        /// Updates the sensor list.
        /// </summary>
        /// <param name="supportedPids">The supported PIDs.</param>
        private void UpdateSensorList(uint supportedPids)
        {
            try
            {
                this.listViewSensor.Items.Clear();

                ListViewItem item;
                if (ObdSupport.IsPidSupported(supportedPids, 4, 0))
                {
                    item = new ListViewItem("Engine load");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 5, 0))
                {
                    item = new ListViewItem("Engine coolant temperature");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                item = new ListViewItem("Battery Voltage");
                item.SubItems.Add("<No Value>");
                item.Checked = true;
                this.listViewSensor.Items.Add(item);

                if (ObdSupport.IsPidSupported(supportedPids, 0x0c, 0))
                {
                    item = new ListViewItem("Engine RPM");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x0d, 0))
                {
                    item = new ListViewItem("Vehicle Speed");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x0f, 0))
                {
                    item = new ListViewItem("Intake air temperature");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x11, 0))
                {
                    item = new ListViewItem("Throttle position");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x1F, 0))
                {
                    item = new ListViewItem("Run time since engine start");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x2F, 0x20))
                {
                    item = new ListViewItem("Fuel Level Input");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x33, 0x20))
                {
                    item = new ListViewItem("Barometric pressure");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x43, 0x40))
                {
                    item = new ListViewItem("Absolute load value");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x45, 0x40))
                {
                    item = new ListViewItem("Relative throttle position");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x46, 0x40))
                {
                    item = new ListViewItem("Ambient air temperature");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x47, 0x40))
                {
                    item = new ListViewItem("Absolute throttle position B");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if

                if (ObdSupport.IsPidSupported(supportedPids, 0x4C, 0x40))
                {
                    item = new ListViewItem("Commanded throttle actuator");
                    item.SubItems.Add("<No Value>");
                    item.Checked = true;
                    this.listViewSensor.Items.Add(item);
                } // if
            }
            catch (Exception ex)
            {
                log.Error("Error updating sensor list", ex);
            } // catch
        } // UpdateSensorList()

        /// <summary>
        /// Lists the capabilities.
        /// </summary>
        private void ListCapabilities()
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                this.resultBuilder = new StringBuilder(1000);
                this.obdManager.ListCapabilities(this.OutputResultBuilder);
                var resultForm = new ResultForm(this.resultBuilder.ToString());
                resultForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.Error("Error listing capabilities", ex);
            } // catch
        } // ListCapabilities()

        /// <summary>
        /// Creates the raw report.
        /// </summary>
        private void CreateRawReport()
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                this.resultBuilder = new StringBuilder(1000);
                this.obdManager.CreateRawReport(this.OutputResultBuilder);
                var resultForm = new ResultForm(this.resultBuilder.ToString());
                resultForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.Error("Error creating raw report", ex);
            } // catch
        } // CreateRawReport()

        /// <summary>
        /// Outputs the specified text to the log.
        /// </summary>
        /// <param name="text">The text.</param>
        private void OutputResultBuilder(string text)
        {
            this.resultBuilder.AppendLine(text);
        } // OutputResultBuilder()

        /// <summary>
        /// Handles the refresh.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance 
        /// containing the event data.</param>
        private void RefreshTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (this.tabControl.SelectedIndex == 0)
            {
                this.RefreshDashboard();
                return;
            } // if

            if ((this.tabControl.SelectedIndex == 1)
                && this.checkAutoRefreshSensorData.Checked)
            {
                this.RefreshSensorData();
            } // if
        } // RefreshTimerOnTick()

        /// <summary>
        /// Refreshes the dashboard.
        /// </summary>
        private async void RefreshDashboard()
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                var speed = await this.obdManager.GetVehicleSpeed();
                this.gaugeSpeed.Value = speed;
                this.lblDashSpeed.Text = string.Format("Speed = {0} km/h", speed);

                var rpm = await this.obdManager.GetEngineRpm();
                this.gaugeEngineSpeed.Value = rpm;
                this.lblDashRpm.Text = string.Format("{0} rpm", rpm);
            }
            catch (Exception ex)
            {
                log.Error("Error refreshing dashboard", ex);
            } // catch
        } // RefreshDashboard()

        /// <summary>
        /// Open Connection to system.
        /// </summary>
        /// <param name="port">The port.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok for top level methods.")]
        private async void OpenConnection(string port)
        {
            if (port == SimulatorPort)
            {
                var simulator = new SerialPortObdSimulator();
                simulator.ShowLogging = false;
                this.lowcomConfig = simulator;
            }
            else
            {
                this.lowcomConfig = new LowLevelSerialPort();
                this.lowcomConfig.PortName = port;
            } // if

            this.obdManager = new ObdManager(this.lowcomConfig);

            try
            {
                await this.obdManager.OpenConnection();
            }
            catch (Exception ex)
            {
                log.Error("Unable to connect: ", ex);
                MessageBox.Show("Unable to connect: " + ex.Message);
            } // catch
        } // OpenConnection()

        /// <summary>
        /// Close connection to system.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok for top level methods.")]
        private void CloseConnection()
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                this.obdManager.CloseConnection();
            }
            catch (Exception ex)
            {
                log.Error("Unable to disconnect: ", ex);
                MessageBox.Show("Unable to disconnect: " + ex.Message);
            } // catch
        } // CloseConnection()

        /// <summary>
        /// Updates the UI.
        /// </summary>
        private void UpdateUI()
        {
            var connected = false;
            if (this.obdManager != null)
            {
                connected = this.obdManager.IsConnected;
            } // if

            this.menuFileStart.Enabled = !connected;
            this.menuFileStop.Enabled = connected;
            this.toolBtnStart.Enabled = !connected;
            this.toolBtnStop.Enabled = connected;
            this.toolComboPorts.Enabled = !connected;

            this.btnGetBatteryVoltage.Enabled = connected;
            this.btnSendRawCommand.Enabled = connected;
            this.btnSendPidCommand.Enabled = connected;
            this.btnCreateRawReport.Enabled = connected;
            this.btnCreateFullReport.Enabled = connected;
            this.btnListCapabilities.Enabled = connected;
            this.checkAutoRefreshSensorData.Enabled = connected;
            this.btnRefreshSensorData.Enabled = !this.checkAutoRefreshSensorData.Checked
                && connected;
            this.btnSaveStatistics.Enabled = connected;
            this.btnReadDtc.Enabled = connected;
            this.btnClearDtcCodes.Enabled = connected;

            this.statusStripLabelConnected.Text = connected 
                ? "Connected to " + this.lowcomConfig.PortName : "Not connected";
        } // UpdateUI()

        /// <summary>
        /// Initializes the user interface.
        /// </summary>
        private void InitUI()
        {
            // initialize icon display
            var assem = this.GetType().Assembly;
            var stream = assem.GetManifestResourceStream(
                "Tethys.OBD.ObdAnalyzer.Net.OBD.ico");
            if (stream != null)
            {
                this.Icon = new Icon(stream, 32, 32);
            } // if

            // initializes debug trace window
            this.rtfLogView.AddAtTail = true;
#if DEBUG
            this.rtfLogView.ShowDebugEvents = true;
            this.rtfLogView.MaxLogLength = 20000;
#else
            this.debugTraceForm.RtfLogView.MaxLogLength = 10000;
#endif

            // toolbar item menu links
            this.toolBtnStart.Tag = this.menuFileStart;
            this.toolBtnStop.Tag = this.menuFileStop;
            this.toolBtnAbout.Tag = this.menuHelpAbout;

            this.InitializeListView();
            this.checkAutoRefreshSensorData.Checked = true;
        } // InitUI()

        /// <summary>
        /// Initializes the ListView.
        /// </summary>
        private void InitializeListView()
        {
            var ch = new ColumnHeader();
            ch.Text = "Sensor";
            ch.Width = 250;
            this.listViewSensor.Columns.Add(ch);

            ch = new ColumnHeader();
            ch.Text = "Value";
            ch.Width = 100;
            this.listViewSensor.Columns.Add(ch);

            ch = new ColumnHeader();
            ch.Text = "DTC";
            ch.Width = 100;
            this.listViewDtc.Columns.Add(ch);

            ch = new ColumnHeader();
            ch.Text = "Description";
            ch.Width = 300;
            this.listViewDtc.Columns.Add(ch);
        } // InitializeListView()

        /// <summary>
        /// Initializes the COM port list.
        /// </summary>
        private void InitializesComPortList()
        {
            var portlist = ComPortSupport.GetPortNames(true);

            if (portlist != null)
            {
                // ReSharper disable CoVariantArrayConversion
                this.toolComboPorts.Items.AddRange(portlist);
                // ReSharper restore CoVariantArrayConversion
            } // if

            this.toolComboPorts.Items.Add(SimulatorPort);

            if (this.toolComboPorts.Items.Count > 0)
            {
                this.toolComboPorts.SelectedIndex = 0;
            } // if
        } // InitializesComPortList()

        /// <summary>
        /// Configures the logging.
        /// </summary>
        private void ConfigureLogging()
        {
            LogManager.Adapter = new LogViewFactoryAdapter(this.rtfLogView);
            log = LogManager.GetLogger(typeof(MainForm));
        } // ConfigureLogging()
        #endregion // PRIVATE METHODS
      } // MainForm
}
