#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Bluetooth test application.
//
// ==========================================================================
// <copyright file="MainPage.xaml.cs" company="Tethys">
// Copyright  2014 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Microsoft Windows Phone 8
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.BluetoothTest
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Tethys.OBD.BluetoothTest.Core;

    using Windows.Networking.Proximity;

    /// <summary>
    /// The main page.
    /// </summary>
    public partial class MainPage
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The bluetooth manager.
        /// </summary>
        private readonly BluetoothManager bluetoothManager;

        /// <summary>
        /// The devices.
        /// </summary>
        private List<PeerInformation> devices;

        /// <summary>
        /// The obd manager
        /// </summary>
        private ObdManager obdManager;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.bluetoothManager = new BluetoothManager();
            this.bluetoothManager.OutputHandler = this.OutputText;
            this.bluetoothManager.MessageBoxReporter = ShowMessageBox;

            this.EnablePanoramaPages(false);
        } // MainPage()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// Handles the OnClick event of the CheckBluetooth control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBluetooth_OnClick(object sender, RoutedEventArgs e)
        {
            this.bluetoothManager.CheckForBluetooth();
        } // CheckBluetooth_OnClick()

        /// <summary>
        /// Handles the OnClick event of the <c>BtnListDevices</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void BtnListDevices_OnClick(object sender, RoutedEventArgs e)
        {
            this.devices = await this.bluetoothManager.FindBluetoothDevices();

            this.ListBoxDevices.Items.Clear();
            foreach (var peerInformation in this.devices)
            {
                var displayText = string.Format("{0} {1}", 
                    peerInformation.DisplayName, peerInformation.HostName);

                this.OutputText(displayText);
                this.ListBoxDevices.Items.Add(displayText);
            } // foreach
        } // BtnListDevices_OnClick()

        /// <summary>
        /// Handles the OnClick event of the <c>BtnConnect</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void BtnConnect_OnClick(object sender, RoutedEventArgs e)
        {
            var index = this.ListBoxDevices.SelectedIndex;
            if (index < 0)
            {
                return;
            } // if

            var peerInfo = this.devices[index];
            var socket = await this.bluetoothManager.ConnectToDevice(peerInfo);
            if (socket != null)
            {
                this.OutputText("Connected.");
                this.obdManager = new ObdManager(socket);
                this.EnablePanoramaPages(true);
                this.MainPanorama.DefaultItem = this.MainPanorama.Items[1];
                this.TxtSend.Text = "ATI";
            }
            else
            {
                ShowMessageBox("Connection to OBD Interface failed");
            } // if
        } // BtnConnect_OnClick()

        /// <summary>
        /// Handles the OnClick event of the <c>BtnSend</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void BtnSendOnClick(object sender, RoutedEventArgs e)
        {
            this.TxtSendReply.Text = string.Empty;

            var send = this.TxtSend.Text;
            if ((string.IsNullOrEmpty(send)) || (this.obdManager == null))
            {
                return;
            } // if

            this.TxtSendReply.Text = "Sending command " + send + "\r\n";

            // only with CRLF attached it is a valid command
            send = send + "\r\n";

#if true
            var text = await this.obdManager.SendRawCommand(send);
            this.TxtSendReply.Text += text + "\r\n";
#else
            var result = await this.obdManager.WriteRaw(send);
            if (result == null)
            {
                return;
            } // if
            if (result[0] == 0xff)
            {
                this.OutputText("Error receiving data!\r\n");
                return;
            } // if


            var text = Encoding.UTF8.GetString(result, 0, FindFirstNull(result));
            this.OutputText(text);

            var hexText = string.Empty;
            foreach (var b in result)
            {
                hexText += string.Format("{0:X2} ", b);
            } // foreach

            this.TxtReply.Text += text + "\r\n" + hexText + "\r\n";
#endif
        } // BtnSendOnClick()

        /// <summary>
        /// Handles the OnClick event of the <c>BtnClear</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance
        /// containing the event data.</param>
        private void BtnClearOnClick(object sender, RoutedEventArgs e)
        {
            this.TxtSendReply.Text = string.Empty;
        } // BtnClearOnClick()
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Enables the panorama pages.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        private void EnablePanoramaPages(bool enable)
        {
            ((Control)this.MainPanorama.Items[1]).IsEnabled = enable;
            ((Control)this.MainPanorama.Items[2]).IsEnabled = enable;
        } // EnablePanoramaPages()

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="text">The text.</param>
        private static void ShowMessageBox(string text)
        {
            MessageBox.Show(text);
        } // ShowMessageBox()

        /// <summary>
        /// Outputs the text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void OutputText(string text)
        {
            this.TxtDeviceStatus.Text += text + "\r\n";
        } // OutputText()
        #endregion // PRIVATE METHODS
    }
}