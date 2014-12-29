#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A support library for Windows Forms applications.
//
// ==========================================================================
// <copyright file="AboutBox.cs" company="Tethys">
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

namespace Tethys.OBD.ObdAnalyzer.Net.UI
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A reusable about box window.
    /// </summary>
    public partial class AboutBox : Form
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public Image Image { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutBox"/> class.
        /// </summary>
        public AboutBox()
        {
            this.InitializeComponent();
        } // AboutBox()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// Handles the Load event of the AboutBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance
        /// containing the event data.</param>
        private void AboutBoxLoad(object sender, System.EventArgs e)
        {
            this.Text = this.Title;
            this.lblTitle.Text = this.Title;
            this.lblVersion.Text = this.Version;
            this.lblCopyright.Text = this.Copyright;
            this.txtDescription.Text = this.Description;
            this.pictureBox.Image = this.Image;
        } // AboutBoxLoad()
        #endregion // PUBLIC METHODS
    } // AboutBox
} // Tethys.OBD.ObdAnalyzer.Net.UI
