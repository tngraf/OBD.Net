namespace Tethys.UiSupport
{
  partial class ExceptionHandlerForm
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
      if (disposing && (this.components != null))
      {
        this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionHandlerForm));
            this.lblException = new System.Windows.Forms.Label();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupError = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupError.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblException
            // 
            resources.ApplyResources(this.lblException, "lblException");
            this.lblException.Name = "lblException";
            // 
            // btnIgnore
            // 
            resources.ApplyResources(this.btnIgnore, "btnIgnore");
            this.btnIgnore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.UseVisualStyleBackColor = true;
            // 
            // txtDetails
            // 
            resources.ApplyResources(this.txtDetails, "txtDetails");
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            // 
            // pictureBox
            // 
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            // 
            // btnRetry
            // 
            resources.ApplyResources(this.btnRetry, "btnRetry");
            this.btnRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupError
            // 
            resources.ApplyResources(this.groupError, "groupError");
            this.groupError.Controls.Add(this.btnSave);
            this.groupError.Controls.Add(this.txtDetails);
            this.groupError.Name = "groupError";
            this.groupError.TabStop = false;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // ExceptionHandlerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupError);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnIgnore);
            this.Controls.Add(this.lblException);
            this.Name = "ExceptionHandlerForm";
            this.ShowIcon = false;
            this.Activated += new System.EventHandler(this.ExceptionHandlerForm_Activated);
            this.Load += new System.EventHandler(this.ExceptionHandlerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupError.ResumeLayout(false);
            this.groupError.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblException;
    private System.Windows.Forms.Button btnIgnore;
    private System.Windows.Forms.TextBox txtDetails;
    private System.Windows.Forms.PictureBox pictureBox;
    private System.Windows.Forms.Button btnRetry;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.GroupBox groupError;
    private System.Windows.Forms.Button btnSave;
  }
}