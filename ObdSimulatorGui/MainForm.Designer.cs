namespace Tethys.OBD.ObdSimulator.UI
{
    partial class MainForm
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.comboPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupCommunication = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.groupSettings = new System.Windows.Forms.GroupBox();
            this.txtVehicleSpeed = new System.Windows.Forms.TextBox();
            this.txtEngineRpm = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEngineCoolantTemperature = new System.Windows.Forms.TextBox();
            this.txtEngineLoad = new System.Windows.Forms.TextBox();
            this.btnSetValues = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtfLogView = new Tethys.Logging.Controls.RtfLogView();
            this.groupCommunication.SuspendLayout();
            this.groupSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(449, 23);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnectClick);
            // 
            // comboPort
            // 
            this.comboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPort.FormattingEnabled = true;
            this.comboPort.Location = new System.Drawing.Point(41, 23);
            this.comboPort.Name = "comboPort";
            this.comboPort.Size = new System.Drawing.Size(121, 21);
            this.comboPort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port:";
            // 
            // groupCommunication
            // 
            this.groupCommunication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCommunication.Controls.Add(this.btnDisconnect);
            this.groupCommunication.Controls.Add(this.label1);
            this.groupCommunication.Controls.Add(this.btnConnect);
            this.groupCommunication.Controls.Add(this.comboPort);
            this.groupCommunication.Location = new System.Drawing.Point(12, 12);
            this.groupCommunication.Name = "groupCommunication";
            this.groupCommunication.Size = new System.Drawing.Size(611, 57);
            this.groupCommunication.TabIndex = 3;
            this.groupCommunication.TabStop = false;
            this.groupCommunication.Text = " Communication";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisconnect.Location = new System.Drawing.Point(530, 23);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnectClick);
            // 
            // groupSettings
            // 
            this.groupSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSettings.Controls.Add(this.txtVehicleSpeed);
            this.groupSettings.Controls.Add(this.txtEngineRpm);
            this.groupSettings.Controls.Add(this.label4);
            this.groupSettings.Controls.Add(this.label5);
            this.groupSettings.Controls.Add(this.txtEngineCoolantTemperature);
            this.groupSettings.Controls.Add(this.txtEngineLoad);
            this.groupSettings.Controls.Add(this.btnSetValues);
            this.groupSettings.Controls.Add(this.label3);
            this.groupSettings.Controls.Add(this.label2);
            this.groupSettings.Location = new System.Drawing.Point(12, 75);
            this.groupSettings.Name = "groupSettings";
            this.groupSettings.Size = new System.Drawing.Size(611, 100);
            this.groupSettings.TabIndex = 4;
            this.groupSettings.TabStop = false;
            this.groupSettings.Text = " Settings ";
            // 
            // txtVehicleSpeed
            // 
            this.txtVehicleSpeed.Location = new System.Drawing.Point(322, 58);
            this.txtVehicleSpeed.Name = "txtVehicleSpeed";
            this.txtVehicleSpeed.Size = new System.Drawing.Size(100, 20);
            this.txtVehicleSpeed.TabIndex = 12;
            // 
            // txtEngineRpm
            // 
            this.txtEngineRpm.Location = new System.Drawing.Point(322, 22);
            this.txtEngineRpm.Name = "txtEngineRpm";
            this.txtEngineRpm.Size = new System.Drawing.Size(100, 20);
            this.txtEngineRpm.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(237, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Vehicle Speed:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Engine RPM:";
            // 
            // txtEngineCoolantTemperature
            // 
            this.txtEngineCoolantTemperature.Location = new System.Drawing.Point(131, 55);
            this.txtEngineCoolantTemperature.Name = "txtEngineCoolantTemperature";
            this.txtEngineCoolantTemperature.Size = new System.Drawing.Size(100, 20);
            this.txtEngineCoolantTemperature.TabIndex = 8;
            // 
            // txtEngineLoad
            // 
            this.txtEngineLoad.Location = new System.Drawing.Point(131, 22);
            this.txtEngineLoad.Name = "txtEngineLoad";
            this.txtEngineLoad.Size = new System.Drawing.Size(100, 20);
            this.txtEngineLoad.TabIndex = 7;
            // 
            // btnSetValues
            // 
            this.btnSetValues.Location = new System.Drawing.Point(530, 71);
            this.btnSetValues.Name = "btnSetValues";
            this.btnSetValues.Size = new System.Drawing.Size(75, 23);
            this.btnSetValues.TabIndex = 6;
            this.btnSetValues.Text = "Set Values";
            this.btnSetValues.UseVisualStyleBackColor = true;
            this.btnSetValues.Click += new System.EventHandler(this.BtnSetValuesClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Engine Coolant Temp.:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Engine Load:";
            // 
            // rtfLogView
            // 
            this.rtfLogView.AddAtTail = true;
            this.rtfLogView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfLogView.LabelText = "Status:";
            this.rtfLogView.Location = new System.Drawing.Point(12, 181);
            this.rtfLogView.MaxLogLength = -1;
            this.rtfLogView.Name = "rtfLogView";
            this.rtfLogView.ShowDebugEvents = false;
            this.rtfLogView.Size = new System.Drawing.Size(611, 232);
            this.rtfLogView.TabIndex = 5;
            this.rtfLogView.TextColor = System.Drawing.Color.Black;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 425);
            this.Controls.Add(this.rtfLogView);
            this.Controls.Add(this.groupSettings);
            this.Controls.Add(this.groupCommunication);
            this.Name = "MainForm";
            this.Text = "OBD Simulator GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.groupCommunication.ResumeLayout(false);
            this.groupCommunication.PerformLayout();
            this.groupSettings.ResumeLayout(false);
            this.groupSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox comboPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupCommunication;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.GroupBox groupSettings;
        private Tethys.Logging.Controls.RtfLogView rtfLogView;
        private System.Windows.Forms.TextBox txtEngineLoad;
        private System.Windows.Forms.Button btnSetValues;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVehicleSpeed;
        private System.Windows.Forms.TextBox txtEngineRpm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEngineCoolantTemperature;
    }
}

