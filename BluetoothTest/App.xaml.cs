#region Header
// --------------------------------------------------------------------------
// OBD Tools
// ==========================================================================
//
// Bluetooth test application.
//
// ==========================================================================
// <copyright file="App.xaml.cs" company="Tethys">
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
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Navigation;

    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    using Tethys.OBD.BluetoothTest.Resources;

    /// <summary>
    /// The app.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Gets the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            this.UnhandledException += this.OnApplicationUnhandledException;

            // Standard XAML initialization
            this.InitializeComponent();

            // Phone-specific initialization
            this.InitializePhoneApplication();

            // Language display initialization
            this.InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                // Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                // Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        /// <summary>
        /// Code to execute when the application is launching (e.g., from Start).
        /// This code will not execute when the application is reactivated
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LaunchingEventArgs"/> instance
        /// containing the event data.</param>
        private void ApplicationLaunching(object sender, LaunchingEventArgs e)
        {
        } // ApplicationLaunching()

        /// <summary>
        /// Code to execute when the application is activated (brought to foreground)
        /// This code will not execute when the application is first launched
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ActivatedEventArgs"/> instance
        /// containing the event data.</param>
        private void ApplicationActivated(object sender, ActivatedEventArgs e)
        {
        } // ApplicationActivated()

        /// <summary>
        /// Code to execute when the application is deactivated (sent to background).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DeactivatedEventArgs"/> instance
        /// containing the event data.</param>
        private void ApplicationDeactivated(object sender, DeactivatedEventArgs e)
        {
        } // ApplicationDeactivated()

        /// <summary>
        /// Code to execute when the application is closing (e.g., user hit Back).
        /// This code will not execute when the application is deactivated
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ClosingEventArgs"/> instance
        /// containing the event data.</param>
        private void ApplicationClosing(object sender, ClosingEventArgs e)
        {
        } // ApplicationClosing()

        /// <summary>
        /// Code to execute if a navigation fails.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NavigationFailedEventArgs"/> instance 
        /// containing the event data.</param>
        private void OnRootFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            } // if
        } // OnRootFrameNavigationFailed()

        /// <summary>
        /// Code to execute on Unhandled Exceptions
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ApplicationUnhandledExceptionEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnApplicationUnhandledException(object sender, 
            ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            } // if
        } // OnApplicationUnhandledException

        #region Phone application initialization

        /// <summary>
        /// Fag to avoid double-initialization.
        /// </summary>
        private bool phoneApplicationInitialized = false;

        /// <summary>
        /// Do not add any additional code to this method.
        /// </summary>
        private void InitializePhoneApplication()
        {
            if (this.phoneApplicationInitialized)
            {
                return;
            } // if

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += this.CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += this.OnRootFrameNavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += this.CheckForResetNavigation;

            // Ensure we don't initialize again
            this.phoneApplicationInitialized = true;
        } // InitializePhoneApplication()

        /// <summary>
        /// Do not add any additional code to this method
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance 
        /// containing the event data.</param>
        private void CompleteInitializePhoneApplication(object sender, 
            NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (this.RootVisual != RootFrame)
            {
                this.RootVisual = RootFrame;
            } // if

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= this.CompleteInitializePhoneApplication;
        } // CompleteInitializePhoneApplication()

        /// <summary>
        /// Checks for reset navigation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance 
        /// containing the event data.</param>
        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
            {
                RootFrame.Navigated += this.ClearBackStackAfterReset;
            } // if
        } // CheckForResetNavigation()

        /// <summary>
        /// Clears the back stack after reset.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance 
        /// containing the event data.</param>
        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= this.ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
            {
                return;
            } // if

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                // do nothing
            } // while
        } // ClearBackStackAfterReset()
        #endregion

        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        
        /// <summary>
        /// Initialize the app's font and flow direction as defined in its localized resource strings.
        /// </summary>
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                } // if

                throw;
            }
        }
    }
}