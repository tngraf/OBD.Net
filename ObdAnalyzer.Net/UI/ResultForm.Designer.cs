namespace Tethys.OBD.ObdAnalyzer.Net.UI
{
    partial class ResultForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtfResult = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtfResult
            // 
            this.rtfResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfResult.Location = new System.Drawing.Point(0, 0);
            this.rtfResult.Name = "rtfResult";
            this.rtfResult.Size = new System.Drawing.Size(498, 574);
            this.rtfResult.TabIndex = 0;
            this.rtfResult.Text = "";
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 574);
            this.Controls.Add(this.rtfResult);
            this.Name = "ResultForm";
            this.Text = "ResultForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtfResult;
    }
}