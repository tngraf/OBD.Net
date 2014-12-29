#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// OBD via Bluetooth application for Windows Phone 8.0 and later.
//
// ==========================================================================
// <copyright file="LandscapePage.xaml.cs" company="Tethys">
// Copyright  2014 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Windows Phone 8
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.OBD.ObdAnalyzer.Views
{
    using System;

    using Microsoft.Phone.Controls;

    using Tethys.OBD.ObdAnalyzer;

    /// <summary>
    /// Landscape page for dashboard display.
    /// </summary>
    public partial class LandscapePage
    {
        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="LandscapePage"/> class.
        /// </summary>
        public LandscapePage()
        {
            this.InitializeComponent();
            this.DataContext = App.ViewModel;
            this.OrientationChanged += this.OnOrientationChanged;
        } // LandscapePage()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING METHODS
        /// <summary>
        /// Called when the orientation has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The
        ///  <see cref="Microsoft.Phone.Controls.OrientationChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if ((e.Orientation == PageOrientation.Portrait)
              || (e.Orientation == PageOrientation.PortraitDown)
              || (e.Orientation == PageOrientation.PortraitUp))
            {
                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                    return;
                } // if

                this.NavigationService.Navigate(new Uri("/Views/MainPage.xaml",
                    UriKind.Relative));
            } // if
        } // OnOrientationChanged()
        #endregion // UI HANDLING METHODS
    } // LandscapePage
} // Tethys.OBD.ObdAnalyzer.Views