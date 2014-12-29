#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// OBD-Analyzer for Windows Phone.
//
// ==========================================================================
// <copyright file="DeviceViewModel.cs" company="Tethys">
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

namespace Tethys.OBD.ObdAnalyzer.ViewModels
{
    using System.ComponentModel;
    using System.Globalization;

    using Tethys.Silverlight.MVVM;

    using Windows.Networking.Proximity;

    /// <summary>
    /// View model for bluetooth devices.
    /// </summary>
    public class DeviceViewModel : ViewModelBase
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The display name.
        /// </summary>
        private string displayName;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }

            set
            {
                if (value != this.displayName)
                {
                    this.displayName = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The host name.
        /// </summary>
        private string hostName;

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        public string HostName
        {
            get
            {
                return this.hostName;
            }

            set
            {
                if (value != this.hostName)
                {
                    this.hostName = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the peer information.
        /// </summary>
        public PeerInformation PeerInformation { get; private set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceViewModel"/> class.
        /// </summary>
        public DeviceViewModel()
        {
            this.displayName = string.Empty;
            this.hostName = string.Empty;

            if (DesignerProperties.IsInDesignTool)
            {
                this.displayName = "My Super OSB Device";
                this.hostName = "(00:11:22:33:44:55)";
            } // if
        } // DeviceViewModel()

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceViewModel"/> class.
        /// </summary>
        /// <param name="displayname">The display name.</param>
        /// <param name="hostname">The hostname.</param>
        public DeviceViewModel(string displayname, string hostname)
        {
            this.displayName = displayname;
            this.hostName = hostname;
        } // DeviceViewModel()

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceViewModel" /> class.
        /// </summary>
        /// <param name="peerinfo">The peer info.</param>
        public DeviceViewModel(PeerInformation peerinfo)
        {
            this.DisplayName = peerinfo.DisplayName;
            this.HostName = peerinfo.HostName.ToString();
            this.PeerInformation = peerinfo;
        } // DeviceViewModel()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0} {1}",
                this.displayName, this.hostName);
        } // ToString()
        #endregion // PUBLIC METHODS
    } // DeviceViewModel
} // Tethys.OBD.ObdAnalyzer.ViewModels