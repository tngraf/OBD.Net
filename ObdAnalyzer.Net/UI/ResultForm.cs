#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A support library for Windows Forms applications.
//
// ==========================================================================
// <copyright file="ResultForm.cs" company="Tethys">
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
    using System.Windows.Forms;

    /// <summary>
    /// A form to display a text result.
    /// </summary>
    public partial class ResultForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultForm"/> class.
        /// </summary>
        public ResultForm()
        {
            this.InitializeComponent();
        } // ResultForm()

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultForm"/> class.
        /// </summary>
        /// <param name="text">The text displayed by the control.</param>
        public ResultForm(string text)
        {
            this.InitializeComponent();

            this.rtfResult.Text = text;
        } // ResultForm()
    } // ResultForm
} // Tethys.OBD.ObdAnalyzer.Net.UI
