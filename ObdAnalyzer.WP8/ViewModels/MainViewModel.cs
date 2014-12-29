#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// OBD-Analyzer for Windows Phone.
//
// ==========================================================================
// <copyright file="MainViewModel.cs" company="Tethys">
// Copyright  2014 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Windows Phone
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

//// #define USE_SIMULATOR

namespace Tethys.OBD.ObdAnalyzer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Microsoft.Phone.Tasks;

    using Tethys.OBD.ObdAnalyzer.Core;
    using Tethys.Silverlight.MVVM;

    using Windows.Networking.Proximity;

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The refresh time in milliseconds (500).
        /// </summary>
        private const int RefreshTimeDefault = 500;

        /// <summary>
        /// String builder for results.
        /// </summary>
        private readonly StringBuilder resultBuilder;

        /// <summary>
        /// The bluetooth manager.
        /// </summary>
        private BluetoothManager bluetoothManager;

        /// <summary>
        /// The bluetoothDevices.
        /// </summary>
        private List<PeerInformation> bluetoothDevices;

        /// <summary>
        /// The socket connection.
        /// </summary>
        private ObdStreamSocketDeviceConnection socketConnection;

        /// <summary>
        /// The obd manager
        /// </summary>
        private ObdManager obdManager;

        /// <summary>
        /// String builder for reports.
        /// </summary>
        private StringBuilder reportBuilder;

        /// <summary>
        /// The refresh timer.
        /// </summary>
        private DispatcherTimer refreshTimer;

        /// <summary>
        /// The auto-refresh 'was running' flag.
        /// </summary>
        private bool wasRunning;

        /// <summary>
        /// The query done flag.
        /// </summary>
        private bool done = true;

        /// <summary>
        /// The refresh time.
        /// </summary>
        private int refreshTime = RefreshTimeDefault;

        /// <summary>
        /// The synchronization context,
        /// </summary>
        private SynchronizationContext synchronizationContext;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// The pivot index connect page.
        /// </summary>
        public const int PivotIndexConnect = 0;

        /// <summary>
        /// The pivot index test page.
        /// </summary>
        public const int PivotIndexTest = 1;

        /// <summary>
        /// The pivot index reports page.
        /// </summary>
        public const int PivotIndexReports = 2;

        /// <summary>
        /// The pivot index dashboard page.
        /// </summary>
        public const int PivotIndexDashboard = 3;

        /// <summary>
        /// Gets the collection for DeviceViewModel objects.
        /// </summary>
        public ObservableCollection<DeviceViewModel> Devices { get; private set; }

        /// <summary>
        /// The selected device.
        /// </summary>
        private DeviceViewModel selectedDevice;

        /// <summary>
        /// Gets or sets the selected device.
        /// </summary>
        public DeviceViewModel SelectedDevice
        {
            get
            {
                return this.selectedDevice;
            }

            set
            {
                if (value != this.selectedDevice)
                {
                    this.selectedDevice = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The text to send.
        /// </summary>
        private string textToSend;

        /// <summary>
        /// Gets or sets the text to send.
        /// </summary>
        public string TextToSend
        {
            get
            {
                return this.textToSend;
            }

            set
            {
                if (value != this.textToSend)
                {
                    this.textToSend = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the result text.
        /// </summary>
        public string ResultText
        {
            get
            {
                return this.resultBuilder.ToString();
            }
        }

        /// <summary>
        /// Gets the report text.
        /// </summary>
        public string ReportText
        {
            get
            {
                return this.reportBuilder.ToString();
            }
        }

        /// <summary>
        /// The text to display.
        /// </summary>
        private string toastMessage;

        /// <summary>
        /// Gets or sets the toast message.
        /// </summary>
        public string ToastMessage
        {
            get
            {
                return this.toastMessage;
            }

            set
            {
                if (value != this.toastMessage)
                {
                    this.toastMessage = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The show toast message flag.
        /// </summary>
        private Visibility showToastMessage;

        /// <summary>
        /// Gets or sets the show toast message flag.
        /// </summary>
        public Visibility ShowToastMessage
        {
            get
            {
                return this.showToastMessage;
            }

            set
            {
                if (value != this.showToastMessage)
                {
                    this.showToastMessage = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The selected pivot item.
        /// </summary>
        private int selectedPivotItem;

        /// <summary>
        /// Gets or sets the selected pivot item.
        /// </summary>
        public int SelectedPivotItem
        {
            get
            {
                return this.selectedPivotItem;
            }

            set
            {
                if (value != this.selectedPivotItem)
                {
                    this.selectedPivotItem = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The speed.
        /// </summary>
        private int speed;

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        public int Speed
        {
            get
            {
                return this.speed;
            }

            set
            {
                if (value != this.speed)
                {
                    this.speed = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The rpm.
        /// </summary>
        private int rpm;

        /// <summary>
        /// Gets or sets the rpm.
        /// </summary>
        public int Rpm
        {
            get
            {
                return this.rpm;
            }

            set
            {
                if (value != this.rpm)
                {
                    this.rpm = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The coolant temperature.
        /// </summary>
        private int coolantTemp;

        /// <summary>
        /// Gets or sets the coolant temperature.
        /// </summary>
        public int CoolantTemp
        {
            get
            {
                return this.coolantTemp;
            }

            set
            {
                if (value != this.coolantTemp)
                {
                    this.coolantTemp = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The engine load.
        /// </summary>
        private int engineLoad;

        /// <summary>
        /// Gets or sets the engine load.
        /// </summary>
        public int EngineLoad
        {
            get
            {
                return this.engineLoad;
            }

            set
            {
                if (value != this.engineLoad)
                {
                    this.engineLoad = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The intake air temperature.
        /// </summary>
        private int intakeAirTemp;

        /// <summary>
        /// Gets or sets the intake air temperature.
        /// </summary>
        public int IntakeAirTemp
        {
            get
            {
                return this.intakeAirTemp;
            }

            set
            {
                if (value != this.intakeAirTemp)
                {
                    this.intakeAirTemp = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The ambient air temperature.
        /// </summary>
        private int ambientAirTemp;

        /// <summary>
        /// Gets or sets the ambient air temperature.
        /// </summary>
        public int AmbientAirTemp
        {
            get
            {
                return this.ambientAirTemp;
            }

            set
            {
                if (value != this.ambientAirTemp)
                {
                    this.ambientAirTemp = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The start button text.
        /// </summary>
        private string startButtonText;

        /// <summary>
        /// Gets or sets the start button text.
        /// </summary>
        public string StartButtonText
        {
            get
            {
                return this.startButtonText;
            }

            set
            {
                if (value != this.startButtonText)
                {
                    this.startButtonText = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The connected hint.
        /// </summary>
        private string connectedHint;

        /// <summary>
        /// Gets or sets the connected hint.
        /// </summary>
        public string ConnectedHint
        {
            get
            {
                return this.connectedHint;
            }

            set
            {
                if (value != this.connectedHint)
                {
                    this.connectedHint = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Fag whether to show the progress indicator.
        /// </summary>
        private bool showProgress;

        /// <summary>
        /// Gets or sets a value indicating whether to show the progress indicator.
        /// </summary>
        public bool ShowProgress
        {
            get
            {
                return this.showProgress;
            }

            set
            {
                if (value != this.showProgress)
                {
                    this.showProgress = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the loaded command.
        /// </summary>
        public ICommand LoadedCommand { get; private set; }

        /// <summary>
        /// Gets the connect command.
        /// </summary>
        public ICommand ConnectCommand { get; private set; }

        /// <summary>
        /// Gets the send command.
        /// </summary>
        public ICommand SendCommand { get; private set; }

        /// <summary>
        /// Gets the send report command.
        /// </summary>
        public ICommand SendReportCommand { get; private set; }

        /// <summary>
        /// Gets the clear command.
        /// </summary>
        public ICommand ClearCommand { get; private set; }

        /// <summary>
        /// Gets the start command.
        /// </summary>
        public ICommand StartCommand { get; private set; }

        /// <summary>
        /// Gets the full obd report command.
        /// </summary>
        public ICommand FullObdReportCommand { get; private set; }

        /// <summary>
        /// Gets the capabilities report command.
        /// </summary>
        public ICommand CapabilitiesReportCommand { get; private set; }

        /// <summary>
        /// Gets the raw obd report command.
        /// </summary>
        public ICommand RawObdReportCommand { get; private set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.Devices = new ObservableCollection<DeviceViewModel>();

            this.resultBuilder = new StringBuilder(10000);
            this.reportBuilder = new StringBuilder(5000);
            this.InitializeCommands();
            this.ShowToastMessage = Visibility.Collapsed;
            this.speed = 0;
            this.rpm = 0;
            this.coolantTemp = 0;
            this.engineLoad = 0;
            this.startButtonText = "Start";
            this.wasRunning = false;
            this.connectedHint = "Not Connected!";
            this.showProgress = false;
            this.synchronizationContext = SynchronizationContext.Current;

            if (DesignerProperties.IsInDesignTool)
            {
                this.ShowToastMessage = Visibility.Collapsed;
                this.ToastMessage = "Somy dummy message...";
                this.speed = 165;
                this.rpm = 6999;
                this.coolantTemp = 81;
                this.engineLoad = 78;
                this.intakeAirTemp = 13;
                this.ambientAirTemp = 25;
                this.startButtonText = "Start";
                this.showProgress = true;
            } // if
        } // MainViewModel()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitializeCommands()
        {
            this.LoadedCommand = new DelegateCommand(this.ExecuteLoadedCommand);
            this.ConnectCommand = new DelegateCommand(this.ExecuteConnectCommand);
            this.SendCommand = new DelegateCommand(this.ExecuteSendCommand);
            this.ClearCommand = new DelegateCommand(this.ExecuteClearCommand);
            this.StartCommand = new DelegateCommand(this.ExecuteStartCommand);
            this.FullObdReportCommand = new DelegateCommand(this.ExecuteFullObdReportCommand);
            this.CapabilitiesReportCommand = new DelegateCommand(this.ExecuteCapabilitiesReportCommand);
            this.RawObdReportCommand = new DelegateCommand(this.ExecuteRawObdReportCommand);
            this.SendReportCommand = new DelegateCommand(this.ExecuteSendReportCommand);
        } // InitializeCommands()

        /// <summary>
        /// Executes the loaded command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteLoadedCommand(object parameter)
        {
            this.InitBluetooth();
            this.InitBluetoothDeviceList();
        } // ExecuteLoadedCommand()

        /// <summary>
        /// Executes the connect command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
#if USE_SIMULATOR
        private void ExecuteConnectCommand(object parameter)
        {
            this.obdManager = new ObdManager(new ObdSimulatorConnection());
            this.ConnectedHint = "!! Simulator !!";
        } // ExecuteConnectCommand()
#else
        private async void ExecuteConnectCommand(object parameter)
        {
            if (this.selectedDevice == null)
            {
                this.ShowToast("No OBD device selected!");
                return;
            } // if

            var peerInfo = this.SelectedDevice.PeerInformation;
            this.wasRunning = false;
            await this.Connect(peerInfo);

            if (this.SelectedPivotItem == PivotIndexConnect)
            {
#if DEBUG
                this.SelectedPivotItem = PivotIndexTest;
#else
                this.SelectedPivotItem = PivotIndexDashboard;
#endif
                this.TextToSend = "ATI";
            } // if
        } // ExecuteConnectCommand()
#endif

        /// <summary>
        /// Connects to the specified peer.
        /// </summary>
        /// <param name="peerInformation">The peer information.</param>
        /// <returns>An async Task.</returns>
        private async Task Connect(PeerInformation peerInformation)
        {
            var socket = await this.bluetoothManager.ConnectToDevice(peerInformation);
            if (socket == null)
            {
                // stop any running operation
                this.Stop();

                this.ShowToast("Connection to OBD Interface failed");
                return;
            } // if

            this.OutputText("Connected.");
            this.ConnectedHint = "Connected";
            this.socketConnection = new ObdStreamSocketDeviceConnection(socket);
            this.socketConnection.ConnectionClosed += this.OnConnectionClosed;
            this.socketConnection.ErrorEncountered += this.OnErrorEncountered;
            this.obdManager = new ObdManager(this.socketConnection);
            this.obdManager.ErrorEncountered += this.OnErrorEncountered;

            if (this.wasRunning)
            {
                this.Start();
            } // if
        } // Connect()

        /// <summary>
        /// Is called when an error has been encountered.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="s">The string.</param>
        private void OnErrorEncountered(object sender, string s)
        {
            this.resultBuilder.AppendFormat("ERROR: {0}\r\n", s);
        } // OnErrorEncountered()

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        private void Disconnect()
        {
            // stop any running operation
            this.Stop();

            this.socketConnection.ConnectionClosed -= this.OnConnectionClosed;
            this.socketConnection = null;
            this.OutputText("Disconnected.");
            this.ConnectedHint = "Not Connected!";
        } // Disconnect()

        /// <summary>
        /// Called when when the connection has been closed unexpectedly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance 
        /// containing the event data.</param>
        private async void OnConnectionClosed(object sender, EventArgs eventArgs)
        {
            this.wasRunning = (this.refreshTimer != null);
            this.Disconnect();

            // try to reconnect
            if (this.SelectedDevice.PeerInformation == null)
            {
                return;
            } // if

            await this.Connect(this.SelectedDevice.PeerInformation);
        } // OnConnectionClosed()

        /// <summary>
        /// Executes the send command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private async void ExecuteSendCommand(object parameter)
        {
            var send = this.TextToSend;
            if ((string.IsNullOrEmpty(send)) || (this.obdManager == null))
            {
                return;
            } // if

            this.resultBuilder.Append("Sending command " + send + "\r\n");

            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged("ResultText");

            // only with CRLF attached it is a valid command
            send = send + "\r\n";

            var text = await this.obdManager.SendRawCommand(send);
            this.resultBuilder.Append(text + "\r\n");

            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged("ResultText");
        } // ExecuteSendCommand()

        /// <summary>
        /// Executes the clear command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteClearCommand(object parameter)
        {
            this.resultBuilder.Clear();

            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged("ResultText");
        } // ExecuteClearCommand

        /// <summary>
        /// Executes the start command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteStartCommand(object parameter)
        {
            if (this.refreshTimer == null)
            {
                this.Start();
                this.StartButtonText = "Stop";
            }
            else
            {
                this.Stop();
                this.StartButtonText = "Start";
            } // if
        } // ExecuteStartCommand()

        /// <summary>
        /// Executes the full OBD report command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private async void ExecuteFullObdReportCommand(object parameter)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                this.reportBuilder = new StringBuilder(5000);
                await Task.Run(
                    () =>
                    {
                        this.SetProgress(true);
                        this.obdManager.CreateFullReport(this.OutputReportText);
                        this.SetProgress(false);
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error creating full report: " + ex.Message);
            } // catch
        } // ExecuteFullObdReportCommand()

        /// <summary>
        /// Executes the capabilities report command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private async void ExecuteCapabilitiesReportCommand(object parameter)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                this.reportBuilder = new StringBuilder(1000);
                await Task.Run(
                    () =>
                    {
                        this.SetProgress(true);
                        this.obdManager.ListCapabilities(this.OutputReportText);
                        this.SetProgress(false);
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error listing capabilities: " + ex.Message);
            } // catch
        } // ExecuteCapabilitiesReportCommand()

        /// <summary>
        /// Executes the raw obd report command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private async void ExecuteRawObdReportCommand(object parameter)
        {
            if (this.obdManager == null)
            {
                return;
            } // if

            try
            {
                this.reportBuilder = new StringBuilder(20000);
                await Task.Run(
                    () =>
                    {
                        this.SetProgress(true);
                        this.obdManager.CreateRawReport(this.OutputReportText);
                        this.SetProgress(false);
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error creating raw report: " + ex.Message);
            } // catch
        } // ExecuteRawObdReportCommand()

        /// <summary>
        /// Executes the send command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void ExecuteSendReportCommand(object parameter)
        {
            this.SendEmail(this.reportBuilder.ToString());
        } // ExecuteSendReportCommand()

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="data">The data.</param>
        private void SendEmail(string data)
        {
            var emailComposeTask = new EmailComposeTask();

            try
            {
                emailComposeTask.Subject = "OBD-Analyzer Data";
                emailComposeTask.Body = data;

                emailComposeTask.Show();
            }
            catch (Exception ex)
            {
                this.ShowToast("Error sending email: " + ex.Message);
            } // catch
        } // SendEmail()

        /// <summary>
        /// Starts the auto-refresh.
        /// </summary>
        private void Start()
        {
            this.refreshTimer = new DispatcherTimer();
            this.refreshTimer.Tick += this.RefreshTimer;
            this.refreshTimer.Interval = new TimeSpan(0, 0, 0, 0, this.refreshTime);
            this.refreshTimer.Start();
        } // Start()

        /// <summary>
        /// Stops the auto-refresh.
        /// </summary>
        private void Stop()
        {
            if (this.refreshTimer != null)
            {
                this.refreshTimer.Stop();
                this.refreshTimer = null;
            } // if
        } // Stop()

        /// <summary>
        /// Called to refresh the display.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private async void RefreshTimer(object data, EventArgs e)
        {
            if ((this.obdManager == null) || (!this.done))
            {
                return;
            } // if

            var watch = new Stopwatch();
            watch.Start();
            try
            {
                this.done = false;
                this.Speed = await this.obdManager.GetVehicleSpeed();
                this.Rpm = await this.obdManager.GetEngineRpm();
                this.CoolantTemp = await this.obdManager.GetEngineCoolantTemperature();
                this.EngineLoad = await this.obdManager.GetCalculatedEngineLoad();
                this.IntakeAirTemp = await this.obdManager.GetIntakeAirTemperature();
                this.AmbientAirTemp = await this.obdManager.GetAmbientAirTemperature();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error refreshing dashboard: " + ex.Message);
            } // catch
            watch.Stop();
            this.done = true;

            // adjust refresh time if necessary
            if (watch.ElapsedMilliseconds > this.refreshTime + 100)
            {
                this.Stop();
                this.refreshTime = (int)watch.ElapsedMilliseconds + 100;
                this.Start();
            } // if
        } // RefreshTimer()

        /// <summary>
        /// Initializes the bluetooth device list.
        /// </summary>
        private async void InitBluetoothDeviceList()
        {
            if (!this.bluetoothManager.CheckForBluetooth())
            {
                this.ShowToast("Bluetooth is not enabled!");
                return;
            } // if

            this.bluetoothDevices = await this.bluetoothManager.FindBluetoothDevices();
            if ((this.bluetoothDevices == null) || (this.bluetoothDevices.Count == 0))
            {
                this.ShowToast("No bluetooth devices available!");
                return;
            } // if

            this.Devices.Clear();
            foreach (var peerInformation in this.bluetoothDevices)
            {
                var displayText = string.Format("{0} {1}",
                    peerInformation.DisplayName, peerInformation.HostName);
                this.OutputText(displayText);

                this.Devices.Add(new DeviceViewModel(peerInformation));
            } // foreach
        } // InitBluetoothDeviceList()

        /// <summary>
        /// Initializes the bluetooth functionality.
        /// </summary>
        private void InitBluetooth()
        {
            this.bluetoothManager = new BluetoothManager();
            this.bluetoothManager.OutputHandler = this.OutputText;
        } // InitBluetooth()

        /// <summary>
        /// Shows a toast popup with the specified text.
        /// </summary>
        /// <param name="message">The message.</param>
        private async void ShowToast(string message)
        {
            this.ToastMessage = message;

            this.ShowToastMessage = Visibility.Visible;
            await Task.Delay(5000);
            this.ShowToastMessage = Visibility.Collapsed;
        } // ShowToast()

        /// <summary>
        /// Outputs the text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void OutputText(string text)
        {
            this.resultBuilder.Append(text);
            this.resultBuilder.Append("\r\n");

            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged("ResultText");
        } // OutputText()

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        private void SetProgress(bool enable)
        {
            this.synchronizationContext.Post(
                x =>
                {
                    this.ShowProgress = enable;
                }, null);
        } // SetProgress()

        /// <summary>
        /// Outputs the text to the report window.
        /// </summary>
        /// <param name="text">The text.</param>
        private void OutputReportText(string text)
        {
            this.synchronizationContext.Post(
                x =>
                {
                    this.reportBuilder.Append(text);
                    this.reportBuilder.Append("\r\n");

                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.RaisePropertyChanged("ReportText");
                }, null);
        } // OutputText()
        #endregion // PRIVATE METHODS
    } // MainViewModel()
}